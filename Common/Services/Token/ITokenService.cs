using Common.Models.Token;

namespace Common.Services.Token
{
    internal interface ITokenService
    {
        Task<int> Create(RefreshTokenCreate request);
        Task Delete(int refreshTokenId);
        Task<TokenModel?> GetTokenByValue(string value);
    }
}