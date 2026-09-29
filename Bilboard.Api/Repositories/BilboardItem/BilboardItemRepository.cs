using Bilboard.Api.DTO.BilboardItem;
using Bilboard.Api.Resources.BilboardItem;
using Common.Attributes.DependencyInjection;
using Common.Services.Database;

namespace Bilboard.Api.Repositories.BilboardItem
{
    [DependencyInjection]
    public class BilboardItemRepository : IBilboardItemRepository
    {
        private readonly IDatabaseService _databaseService;

        public BilboardItemRepository(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<int> Create(BilboardItemCreateDto request)
        {
            return await _databaseService.Create(Sql.Bilboard_BilboardItem_Create, parameters: request);
        }

        public async System.Threading.Tasks.Task Delete(int bilboardItemId, int userId)
        {
            await _databaseService.Delete(Sql.Bilboard_BilboardItem_Delete, parameters: new { BilboardItemId = bilboardItemId, UserId = userId });
        }

        public async Task<BilboardItemGetDto?> Get(int bilboardItemId)
        {
            return await _databaseService.Get<BilboardItemGetDto>(Sql.Bilboard_BilboardItem_Get, parameters: new { BilboardItemId = bilboardItemId });
        }
    }
}
