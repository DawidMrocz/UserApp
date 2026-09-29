using Common.ApiModels.Access.Request;
using Common.ApiModels.User;
using Common.Models.Authentication;
using Common.Models.OTP;
using Common.Models.User;
using Common.Services.Email;
using Common.Services.OTP;
using Common.Services.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;

namespace Common.Services.Authentication
{
    internal class AuthenticationCookie : IAuthenticationCookie
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly IOtpService _oneTimePasscodeRepository;
        private readonly IEmailService _emailService;
        protected const string LoggedUserKey = "UserSessionKey";

        public AuthenticationCookie(IHttpContextAccessor httpContextAccessor, IUserService userService, IConfiguration configuration, IOtpService oneTimePasscodeRepository, IEmailService emailService)
        {
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
            _configuration = configuration;
            _oneTimePasscodeRepository = oneTimePasscodeRepository;
            _emailService = emailService;
        }

        private string GeneratePasscode()
        {
            var charsTable = Enumerable.Range(0, 10).Select(x => x.ToString()).ToArray();

            var r = new Random();
            var passcore = new StringBuilder();

            for (int i = 0; i < 6; i++)
                passcore.Append(charsTable[r.Next(charsTable.Length)]);

            return passcore.ToString();
        }

        public async Task VerifyOneTimePasscode(Guid guid, string passcode)
        {
            OneTimePasscodeModel? correctPasscode = await _oneTimePasscodeRepository.GetByGuid(guid);

            if (correctPasscode is null || correctPasscode.ExpireDate <= DateTime.Now)
                throw new Exception("Podane hasło jednorazowe jest nieaktualne");

            await _oneTimePasscodeRepository.Delete(correctPasscode.Id);

            if (correctPasscode.Passcode != passcode)
                throw new Exception("Podane hasło jednorazowe jest nieprawidłowe");

            UserModel? user = await _userService.Get(correctPasscode.UserId)
                ?? throw new Exception("User not found");

            List<Claim> claims = [new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()!)];

            ClaimsIdentity identity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new(identity);
            AuthenticationProperties authenticationProperties = new()
            {
                AllowRefresh = true,
                ExpiresUtc = DateTime.Now.AddMinutes(15),
                IsPersistent = correctPasscode.IsAuthTokenPersistent,
            };

            await _httpContextAccessor.HttpContext!.SignInAsync(
                scheme: CookieAuthenticationDefaults.AuthenticationScheme,
                principal: principal,
                properties: authenticationProperties
            );

            _httpContextAccessor.HttpContext!.User = principal;

            string csrfToken = _httpContextAccessor.HttpContext.Session.GetString(LoggedUserKey);

            AuthenticationUser? authenticationUser = new()
            {
                Id = user.Id,
              //  CsrfToken = csrfToken,
                Email = user.Email,
                Culture = user.Culture,
                Roles = user.Roles,
            };

            _httpContextAccessor.HttpContext.Session.SetString(LoggedUserKey, JsonConvert.SerializeObject(authenticationUser));
        }

        public async Task<string> OTPLogin(LoginRequest dto, string? ipAddress, string? userAgent, bool twoFactorAuth, string culture)
        {
            UserModel? user = await _userService.GetByEmail(dto.Email)
                ?? throw new Exception("Nie znaleziono użytkownika lub nie poprawne hasło");

            if (user.UnblockTime is null)
                if (user.Blocked is not null && user.Blocked == true)
                    throw new Exception("Knoto zostało zablokowane");
                else
                if (user.UnblockTime > DateTime.Now)
                    if (user.Blocked is not null && user.Blocked == true)
                        throw new Exception("Knoto zostało zablokowane");

            if (!VerifyPasswordHash(dto.Password, user.PasswordHash, user.PasswordSalt))
                throw new Exception("Nie poprawne hasło");


            string passcode = GeneratePasscode();

            List<MailAddress> mailAddresses = [new MailAddress(user.Email)];

            StringBuilder title = new();

            if (user.FirstName is null)
            {
                title.Append("użytkowniku");
            }
            else
            {
                title.Append($"{user.FirstName} ");
                if (user.LastName is not null)
                    title.Append(user.LastName);
            }

            Dictionary<string, string> bodyParmas = new()
                {
                    { "UserName", title.ToString() },
                    { "Passcode", passcode }
            };

            Dictionary<string, string> subjectParmas = new()
                {
                    { "UserName", title.ToString() },
                };

            await _emailService.SendEmail(mailAddresses, "ToFactorAuth", true, "pl", new MailAddress(_configuration["AppEmail"]), bodyParmas, subjectParmas);

            //DODAC TABELE V
            var passcodeInsert = new OneTimePasscodeModel
            {
                Passcode = passcode,
                UserId = user.Id,
                // Z CONFIGURACJI
                //ExpireDate = DateTime.Now.AddMinutes(_userConf.OneTimePasscodeValidFor),
                ExpireDate = DateTime.Now.AddMinutes(6),
                IsAuthTokenPersistent = dto.RememberMe
            };
            int id = await _oneTimePasscodeRepository.Create(passcodeInsert);
            OneTimePasscodeModel otp = await _oneTimePasscodeRepository.GetById(id)
                ?? throw new Exception("Not found");

            return otp.UserId.ToString();
        }

        public async Task<SessionUser> Login(LoginRequest dto, string? ipAddress, string? userAgent)
        {
            UserModel? user = await _userService.GetByEmail(dto.Email)
                ?? throw new Exception("Nie znaleziono użytkownika lub nie poprawne hasło");

            if (user.UnblockTime is null)
                if (user.Blocked is not null && user.Blocked == true)
                    throw new Exception("Knoto zostało zablokowane");
                else
                if (user.UnblockTime > DateTime.Now)
                    if (user.Blocked is not null && user.Blocked == true)
                        throw new Exception("Knoto zostało zablokowane");

            if (!VerifyPasswordHash(dto.Password, user.PasswordHash, user.PasswordSalt))
                throw new Exception("Nie poprawne hasło");

            List<Claim> claims = [new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()!)];

            ClaimsIdentity identity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new(identity);
            AuthenticationProperties authenticationProperties = new()
            {
                AllowRefresh = true,
                ExpiresUtc = DateTime.Now.AddMinutes(15),
                IsPersistent = dto.RememberMe
            };

            await _httpContextAccessor.HttpContext!.SignInAsync(
                scheme: CookieAuthenticationDefaults.AuthenticationScheme,
                principal: principal,
                properties: authenticationProperties
            );

            _httpContextAccessor.HttpContext!.User = principal;

            SessionUser? sessionUser = new()
            {
                Id = user.Id,
                CsrfToken = Guid.NewGuid().ToString(),
                Email = user.Email,
                Culture = user.Culture ?? _httpContextAccessor.HttpContext!.Request.Cookies["culture"] ?? "pl-PL", 
                Roles = user.Roles,
            };

            _httpContextAccessor.HttpContext.Session.SetString(LoggedUserKey, JsonConvert.SerializeObject(sessionUser));

            return sessionUser;
        }
        private static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (HMACSHA512 hmac = new(passwordSalt))
            {
                byte[] computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        public void Logout()
        {
            _httpContextAccessor.HttpContext?.Session.Remove(LoggedUserKey);
            _httpContextAccessor.HttpContext?.SignOutAsync(scheme: CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public virtual async Task<AuthenticationUser?> GetAuthenticatedUser()
        {
            //Sprawdzenie, czy jest ciastko itp
            HttpContext httpContext = _httpContextAccessor.HttpContext!;
            IIdentity? userIdentity = httpContext.User.Identity;
            if (userIdentity is null || !userIdentity.IsAuthenticated) return default;
            if (userIdentity is WindowsIdentity) return default;
            if (userIdentity is not ClaimsIdentity) return default;

            //Popranie id z ciastka
            string? userIdentifier = ((ClaimsIdentity)userIdentity)!.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            if(string.IsNullOrEmpty(userIdentifier))
                return default;
            int parsedUserIdentifier = int.Parse(userIdentifier);

            //Popbranie info o użytkowniku i odświeżenie czasu sesji
            //----------------------------------------------------------------------------------------------------------------------------------
            UserModel? user = await _userService.Get(parsedUserIdentifier);

            if(user is null)
                return default;

            //Odświeżamy token - napewno jest w sesji, ponieważ ta akcja przeszła już CSRF protection
            string? csrfToken = httpContext.Session.GetString(LoggedUserKey);

            if(csrfToken is null) return default;

            SessionUser? sessionUser = new()
            {
                Id = user.Id,
                CsrfToken = csrfToken,
                Email = user.Email,
                Culture = user.Culture,
                Roles = user.Roles,
            };

            _httpContextAccessor.HttpContext?.Session.SetString(LoggedUserKey, JsonConvert.SerializeObject(sessionUser));

            return new AuthenticationUser()
            {
                Id = user.Id,
                Email = user.Email,
                Culture = user.Culture,
                Roles = user.Roles,
            };
        }
    }
}
