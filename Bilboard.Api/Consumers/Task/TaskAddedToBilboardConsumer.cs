using Bilboard.Api.DTO.BilboardItem;
using Bilboard.Api.DTO.Tasks;
using Bilboard.Api.Repositories.Bilboard;
using Bilboard.Api.Repositories.BilboardItem;
using Bilboard.Api.Repositories.Task;
using MassTransit;
using Models.Rabbit.Task;

namespace Bilboard.Api.Consumers.Task
{
    public class TaskAddedToBilboardConsumer : IConsumer<TaskAddedToBilboardEvent>
    {
        private readonly IBilboardItemRepository _bilboardItemRepository;
        private readonly IBilboardRepository _bilboardRepository;
        private readonly ITaskRepository _taskRepository;

        public TaskAddedToBilboardConsumer(IBilboardRepository bilboardRepository, IBilboardItemRepository bilboardItemRepository, ITaskRepository taskRepository)
        {
            _bilboardRepository = bilboardRepository;
            _bilboardItemRepository = bilboardItemRepository;
            _taskRepository = taskRepository;
        }

        public async System.Threading.Tasks.Task Consume(ConsumeContext<TaskAddedToBilboardEvent> context)
        {
            DTO.Bilboard.BilboardGetItem? bilboard = await _bilboardRepository.Get(context.Message.UserId)
                ?? throw new Exception("Nie znaleziono tablicy");

            TaskGetDto task = await _taskRepository.GetByExternalId(context.Message.TaskId)
                ?? throw new Exception("Nie znaleziono zadania");

            BilboardItemCreateDto request = new()
            {
                TaskId = task.Id,
                BilboardId = bilboard.Id,
                Hours = 0,
                UserId = context.Message.UserId,
            };

            await _bilboardItemRepository.Create(request);
        }
    }
}
