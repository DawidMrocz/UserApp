using Bilboard.Api.DTO.BilboardItem;
using Bilboard.Api.DTO.Tasks;

namespace Bilboard.Api.Repositories.BilboardItem
{
    public interface IBilboardItemRepository
    {
        System.Threading.Tasks.Task Delete(int bilboardItemId, int userId);
        Task<BilboardItemGetDto?> Get(int bilboardItemId);
        Task<int> Create(BilboardItemCreateDto request);
    }
}
