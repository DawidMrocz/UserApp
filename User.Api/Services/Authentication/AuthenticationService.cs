using Common.ApiModels.Access.Request;
using Common.ApiModels.User;
using Common.Attributes.DependencyInjection;
using Common.Models.Token;
using Common.Services.User;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using User.Api.Dto.JwtToken;
using User.Api.Dto.RefreshToken;
using User.Api.Repositories.RefreshToken;

namespace User.Api.Services.Authentication
{
    [DependencyInjection]
    internal class AuthenticationService : IAuthenticationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IConfiguration _configuration;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public AuthenticationService(
            IHttpContextAccessor httpContextAccessor,

            IConfiguration configuration,
            IRefreshTokenRepository refreshTokenRepository,
            IUserService userService)
        {
            _httpContextAccessor = httpContextAccessor;

            _configuration = configuration;
            _refreshTokenRepository = refreshTokenRepository;
            _userService = userService;
        }

        /// <summary>
        /// Pobranie JWT z nagłówka
        /// </summary>
        /// <returns></returns>
        private string? GetJwtTokenFromHeader()
        {
            const string Scheme = "Bearer ";

            if (!_httpContextAccessor.HttpContext!.Request.Headers.TryGetValue("Authorization", out StringValues authorizationHeader))
                return null;

            if (!authorizationHeader.ToString().StartsWith(Scheme, StringComparison.OrdinalIgnoreCase))
                return null;

            return authorizationHeader.ToString().Substring(Scheme.Length).Trim();
        }

        /// <summary>
        /// Pobranie uwierzytelnionego użytkownika
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<AuthenticationJwtUser?> GetAuthenticatedUser()
        {
            string? userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId is null) return default;

            if (int.TryParse(userId, out int parsedUserId))
            {
                Common.Models.User.UserModel user = await _userService.Get(parsedUserId)
                    ?? throw new Exception("Nie znaleziono użytkownika");

                return new AuthenticationJwtUser()
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Roles = user.Roles,
                };
            }
            else
            {
                return default;
            }
        }

        /// <summary>
        /// Odświeżenie tokena
        /// </summary>
        /// <param name="incomingRefreshToken"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<RefreshTokenResponse> RefreshToken(string incomingRefreshToken)
        {
            if (string.IsNullOrWhiteSpace(incomingRefreshToken))
                throw new Exception("Brak ważnego refresh tokena");


            //WERYFIKACJA JWT TOKENA
            string? jwtToken = GetJwtTokenFromHeader()
                ?? throw new Exception("jwt not found");

            //Weryfikacja tokena
            RSA rsa = RSA.Create();
            rsa.ImportRSAPublicKey(
                source: Convert.FromBase64String(_configuration["JWT:JwtPublicKey"] ?? throw new Exception("JWT public key not set")),
                bytesRead: out int _
            );

            RsaSecurityKey rsaPublicKey = new(rsa);
            JwtSecurityTokenHandler tokenHandler = new();

            JwtSecurityToken decryptedToken = tokenHandler.ReadJwtToken(jwtToken);
            string userId = decryptedToken.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

            tokenHandler.ValidateToken(jwtToken, new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidAudience = _configuration["JWT:Audience"],
                ValidateIssuer = true,
                ValidIssuers = [_configuration["JWT:Issuer"]],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = rsaPublicKey,
                RequireExpirationTime = true,
                RequireSignedTokens = true,
            }, out var _);

            //Generujemy lub pobieramy lock dla aktualnego usera, żeby wiele requestów naraz nie spamowało refresh tokenów
            await _semaphore.WaitAsync();

            try
            {
                GetRefreshTokenDto? refreshToken = await _refreshTokenRepository.GetByValue(incomingRefreshToken); // w sql czasowy offset

                //Sprawdzamy, czy refresh token należy do uzytkownika oraz czy i on nie jest przeterminowany
                if (refreshToken is null || refreshToken?.ExpireDate <= DateTime.Now)
                    throw new Exception("Brak ważnego refresh tokena");

                Common.Models.User.UserModel user = await _userService.Get(refreshToken!.UserId)
                    ?? throw new Exception("Nie znaleziono użytkownika");

                //Generujemy nowy jwt token
                JwtTokenDto newJwtToken = CreateJwtToken(user);

                //Usuwamy użyty refresh token
                await _refreshTokenRepository.Delete(refreshToken.Id);

                //Tworzymy nowy refresh token
                CreateRefreshToken newRefreshToken = await CreateRefreshToken(newJwtToken, user.Id);

                return new RefreshTokenResponse()
                {
                    JwtToken = newJwtToken.Value,
                    JwtTokenExpire = newJwtToken.Expire,
                    RefreshToken = newRefreshToken.RefreshTokenValue,
                    RefreshTokenExpire = newRefreshToken.ExpireDate,
                };
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private JwtTokenDto CreateJwtToken(Common.Models.User.UserModel user)
        {
            List<Claim> claims = [
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FirstName!),
                new Claim(ClaimTypes.Surname, user.LastName!)            
                ];

            foreach (Common.Models.Role.RoleModel role in user.Roles)
                claims.Add(new Claim(ClaimTypes.Role, role.StrongName));

            string privateKey = _configuration["JWT:JWTPrivateKey"] ?? throw new Exception("JWT private key not set");

            using RSA rsa = RSA.Create();

            rsa.ImportRSAPrivateKey(
                source: Convert.FromBase64String(privateKey),
                bytesRead: out int _);

            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(privateKey));

            SigningCredentials signingCredentials = new(
               key: new RsaSecurityKey(rsa),
               algorithm: SecurityAlgorithms.RsaSha256)
            {
                CryptoProviderFactory = new CryptoProviderFactory { CacheSignatureProviders = false }
            };

            DateTime expire = DateTime.Now.AddMinutes(15);

            JwtSecurityToken jwt = new(
                audience: _configuration["JWT:Audience"],
                issuer: _configuration["JWT:Issuer"],
                claims: claims,
                notBefore: null,
                expires: expire,
                signingCredentials: signingCredentials
            );

            string token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new JwtTokenDto()
            {
                Expire = expire,
                Value = token
            };
        }

        /// <summary>
        /// Weryfikacja hasła
        /// </summary>
        /// <param name="password"></param>
        /// <param name="passwordHash"></param>
        /// <param name="passwordSalt"></param>
        /// <returns></returns>
        private static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (HMACSHA512 hmac = new(passwordSalt))
            {
                byte[]? computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        /// <summary>
        /// Zalogowanie użytkownika
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<JwtTokenResponse> Login(LoginRequest request)
        {
            //Szukamy usera
            Common.Models.User.UserModel user = await _userService.GetByEmail(request.Email)
                ?? throw new Exception("Nie znaleziono użytkownika lub nie poprawne hasło");

            //Sprawdzamy, czy nie blocked xD
            if (user.UnblockTime is null)
                if (user.Blocked is not null && user.Blocked == true)
                    throw new Exception("Knoto zostało zablokowane");
                else
                if (user.UnblockTime > DateTime.Now)
                    if (user.Blocked is not null && user.Blocked == true)
                        throw new Exception("Knoto zostało zablokowane");

            // Weryfikacja hasła
            if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
                throw new Exception("Nie poprawne hasło");

            //Stworzenie tokena JWT i zapisanie go do sesji użytkownika (bo w sumie to to samo co ciastko, a mam to dostępne w sesji)
            JwtTokenDto jwtToken = CreateJwtToken(user);

            //Stworzenie Refresh tokena w bazie danych dla danego tokena
            CreateRefreshToken refreshToken = await CreateRefreshToken(jwtToken, user.Id);

            //Storzenie tokena CSRF i wysłanie odpowiedzi
            return new JwtTokenResponse()
            {
                JwtToken = jwtToken.Value,
                JwtTokenExpire = jwtToken.Expire,
                RefreshToken = refreshToken.RefreshTokenValue,
                RefreshTokenExpire = refreshToken.ExpireDate,
            };
        }

        /// <summary>
        /// Wylogowanie użytkownika
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task Logout()
        {
            string? jwtToken = GetJwtTokenFromHeader();

            if (string.IsNullOrWhiteSpace(jwtToken))
                throw new Exception("Zaloguj się.");

            GetRefreshTokenDto? refreshToken = await _refreshTokenRepository.GetByJwtToken(jwtToken);

            if (refreshToken is not null)
                await _refreshTokenRepository.Delete(refreshToken.Id);
        }

        /// <summary>
        /// Funbkcja do tworzenia RefreshTokena
        /// </summary>
        /// <param name="jwtToken"></param>
        /// <returns></returns>
        private async Task<CreateRefreshToken> CreateRefreshToken(JwtTokenDto jwtToken, int userId)
        {
            CreateRefreshToken refreshToken = new()
            {
                JwtTokenValue = jwtToken.Value,
                RefreshTokenValue = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                ExpireDate = DateTime.Now.AddDays(1),
                UserId = userId
            };

            await _refreshTokenRepository.Create(refreshToken);

            return refreshToken;
        }

    }
}
