using User.Api.Dto.RefreshToken;
using User.Api.Resources.RefreshToken;

namespace User.Api.Repositories.RefreshToken
{
    public partial class RefreshTokenRepository
    {
        public async Task Create(CreateRefreshToken dto)
        {
            await _databaseService.Create(Sql.User_RefreshToken_Create, parameters: dto);
        }
    }
}
