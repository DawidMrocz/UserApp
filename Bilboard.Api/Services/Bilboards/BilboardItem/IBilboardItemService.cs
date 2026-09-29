using Models.Clients.Tasks;

namespace Bilboard.Api.Services.Bilboards.BilboardItem
{
    public interface IBilboardItemService
    {
        System.Threading.Tasks.Task Delete(int bilboardItemId, int userId);
        System.Threading.Tasks.Task ChangeStatus(int bilboardItemId, TaskChangeStatusRequest request, int userId);
    }
}
