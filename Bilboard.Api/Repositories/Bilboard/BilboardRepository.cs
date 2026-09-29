using Bilboard.Api.DTO.Bilboard;
using Bilboard.Api.Resources.Bilboard;
using Common.Attributes.DependencyInjection;
using Common.Services.Database;

namespace Bilboard.Api.Repositories.Bilboard
{
    [DependencyInjection]
    public class BilboardRepository : IBilboardRepository
    {
        private readonly IDatabaseService _databaseService;

        public BilboardRepository(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<int> Create(int userId)
        {
            return await _databaseService.Create(Sql_Bilboard.Bilboard_Bilboard_Create, parameters: new { UserId = userId });
        }

        public async Task<BilboardGetItem?> Get(int userId)
        {
            return await _databaseService.Get<BilboardGetItem>(Sql_Bilboard.Bilboard_Bilboard_Get, parameters: new { UserId = userId });
        }
    }
}
