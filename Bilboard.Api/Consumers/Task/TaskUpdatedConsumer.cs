using Bilboard.Api.DTO.Tasks;
using Bilboard.Api.Repositories.Task;
using MassTransit;
using Models.Rabbit.Task;

namespace Bilboard.Api.Consumers.Task
{
    public class TaskUpdatedConsumer : IConsumer<TaskUpdatedEvent>
    {
        private readonly ITaskRepository _taskRepository;

        public TaskUpdatedConsumer(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async System.Threading.Tasks.Task Consume(ConsumeContext<TaskUpdatedEvent> context)
        {
            TaskUpdateDto request = new()
            {
                Title = context.Message.Title,
                Deadline = context.Message.Deadline,
                EstimatedHours = context.Message.EstimatedHours,
                IsImportant = context.Message.IsImportant,
                ExternalId = context.Message.TaskId,
                UserId = context.Message.UserId,
            };
            await _taskRepository.Update(request);
        }
    }
}
