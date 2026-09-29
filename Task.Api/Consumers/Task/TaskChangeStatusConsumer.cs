using MassTransit;
using Models.Rabbit.Task;
using Task.Api.Repositories.Tasks.Task;

namespace Task.Api.Consumers.Task
{
    public class TaskChangeStatusConsumer : IConsumer<TaskChangeStatusEvent>
    {
        private readonly ITaskRepository _taskRepository;

        public TaskChangeStatusConsumer(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async System.Threading.Tasks.Task Consume(ConsumeContext<TaskChangeStatusEvent> context)
        {
            await _taskRepository.ChangeStatus(context.Message.TaskId, context.Message.StatusStrongName, context.Message.UserId);
        }
    }
}
