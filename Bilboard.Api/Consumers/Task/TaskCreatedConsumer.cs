using Bilboard.Api.DTO.Tasks;
using Bilboard.Api.Repositories.Task;
using MassTransit;
using Models.Rabbit.Task;

namespace Bilboard.Api.Consumers.Task
{
    public class TaskCreatedConsumer : IConsumer<TaskCreatedEvent>
    {
        private readonly ITaskRepository _taskRepository;

        public TaskCreatedConsumer(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async System.Threading.Tasks.Task Consume(ConsumeContext<TaskCreatedEvent> context)
        {
            TaskCreateDto request = new()
            {
                Title = context.Message.Title,
                Deadline = context.Message.Deadline,
                EstimatedHours = context.Message.EstimatedHours,
                IsImportant = context.Message.IsImportant,
                StatusExternalId = context.Message.StatusExternalId,
                ExternalId = context.Message.TaskId,
                UserId = context.Message.UserId,
            };
            await _taskRepository.Create(request);
        }
    }
}
