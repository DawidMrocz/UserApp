using User.Api.Resources.RefreshToken;

namespace User.Api.Repositories.RefreshToken
{
    public partial class RefreshTokenRepository
    {
        public async Task Delete(int refreshTokenId)
        {
            await _databaseService.Delete(Sql.User_RefreshToken_Delete, parameters: new { RefreshTokenId = refreshTokenId });
        }
    }
}
