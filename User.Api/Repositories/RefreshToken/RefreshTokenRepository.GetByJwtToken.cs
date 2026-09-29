

using User.Api.Dto.RefreshToken;
using User.Api.Resources.RefreshToken;

namespace User.Api.Repositories.RefreshToken
{
    public partial class RefreshTokenRepository
    {
        public async Task<GetRefreshTokenDto?> GetByJwtToken(string jwtToken)
        {
            return await _databaseService.Get<GetRefreshTokenDto>(Sql.User_RefreshToken_GetRefreshTokenByJwtToken, parameters: new { JwtTokenValue = jwtToken });
        }
    }
}
