using Bilboard.Api.DTO.Tasks;
using Bilboard.Api.Repositories.Task;
using MassTransit;
using Models.Rabbit.Task;

namespace Bilboard.Api.Consumers.Task
{
    public class TaskDeletedConsumer : IConsumer<TaskDeletedEvent>
    {
        private readonly ITaskRepository _taskRepository;

        public TaskDeletedConsumer(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async System.Threading.Tasks.Task Consume(ConsumeContext<TaskDeletedEvent> context)
        {
            TaskDeleteDto request = new()
            {
                ExternalId = context.Message.TaskId,
                UserId = context.Message.UserId,
            };
            await _taskRepository.Delete(request);
        }
    }
}
