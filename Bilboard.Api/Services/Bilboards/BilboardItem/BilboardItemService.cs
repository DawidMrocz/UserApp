using Bilboard.Api.DTO.Tasks;
using Bilboard.Api.Repositories.BilboardItem;
using Bilboard.Api.Repositories.Task;
using Common.Attributes.DependencyInjection;
using MassTransit;
using Models.Clients.Tasks;
using Models.Rabbit.Bilboard;
using Models.Rabbit.Task;

namespace Bilboard.Api.Services.Bilboards.BilboardItem
{
    [DependencyInjection]
    public class BilboardItemService : IBilboardItemService
    {
        private readonly IBilboardItemRepository _bilboardItemRepository;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ITaskRepository _taskRepository;

        public BilboardItemService(IBilboardItemRepository bilboardItemRepository, ITaskRepository taskRepository, IPublishEndpoint publishEndpoint)
        {
            _bilboardItemRepository = bilboardItemRepository;
            _taskRepository = taskRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Delete(int bilboardItemId, int userId)
        {
            DTO.BilboardItem.BilboardItemGetDto bilboardItem = await _bilboardItemRepository.Get(bilboardItemId)
                ?? throw new Exception("Item not found");

            await _bilboardItemRepository.Delete(bilboardItem.Id, userId);

            await _publishEndpoint.Publish(new DeleteBilboardItemEvent()
            {
                TaskId = bilboardItem.TaskExternalId,
                UserId = userId,
            });
        }

        public async Task ChangeStatus(int bilboardItemId, TaskChangeStatusRequest request, int userId)
        {
            DTO.BilboardItem.BilboardItemGetDto? bilboardItem = await _bilboardItemRepository.Get(bilboardItemId)
                ?? throw new Exception("Nie znaleziono pozycji na tablicy");

            TaskChangeStatusDto dto = new()
            {
                TaskId = bilboardItem.TaskId,
                UserId = userId,
                StatusStrongName = request.StatusStrongName
            };
            await _taskRepository.ChangeStatus(dto);

            await _publishEndpoint.Publish(new TaskChangeStatusEvent()
            {
                TaskId = bilboardItem.TaskExternalId,
                UserId = userId,
                StatusStrongName = request.StatusStrongName
            });
        }
    }
}
