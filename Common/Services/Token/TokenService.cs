using Common.Models.Token;
using Common.Repositories.Token;
using Common.Services.Database;

namespace Common.Services.Token
{
    internal class TokenService : ITokenService
    {
        private readonly IDatabaseService _databaseService;

        public TokenService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<int> Create(RefreshTokenCreate request)
        {
            return await _databaseService.GetValue<int>(Sql.User_Token_Create, parameters: request);
        }

        public async Task Delete(int tokenId)
        {
            await _databaseService.Delete(Sql.User_Token_Delete, parameters: new { TokenId = tokenId});
        }

        public async Task<TokenModel?> GetTokenByValue(string value)
        {
            return await _databaseService.Get<TokenModel>(Sql.User_Token_GetTokenByValue, parameters: new { Value = value });
        }
    }
}
