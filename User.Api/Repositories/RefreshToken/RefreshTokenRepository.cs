using Common.Services.Database;

namespace User.Api.Repositories.RefreshToken
{
    public partial class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly IDatabaseService _databaseService;

        public RefreshTokenRepository(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }
    }
}
