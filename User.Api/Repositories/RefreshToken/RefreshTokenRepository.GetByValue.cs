

using User.Api.Dto.RefreshToken;
using User.Api.Resources.RefreshToken;

namespace User.Api.Repositories.RefreshToken
{
    public partial class RefreshTokenRepository
    {
        public async Task<GetRefreshTokenDto?> GetByValue(string refreshToken)
        {
            return await _databaseService.Get<GetRefreshTokenDto>(Sql.User_RefreshToken_GetRefreshTokenByValue, parameters: new { RefreshTokenValue = refreshToken });
        }
    }
}
