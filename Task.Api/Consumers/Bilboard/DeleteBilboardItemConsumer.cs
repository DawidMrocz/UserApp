using MassTransit;
using Models.Rabbit.Bilboard;
using Task.Api.Repositories.Tasks.Task;

namespace Task.Api.Consumers.Bilboard
{
    public class DeleteBilboardItemConsumer : IConsumer<DeleteBilboardItemEvent>
    {
        private readonly ITaskRepository _taskRepository;

        public DeleteBilboardItemConsumer(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async System.Threading.Tasks.Task Consume(ConsumeContext<DeleteBilboardItemEvent> context)
        {
            await _taskRepository.RemoveUser(context.Message.TaskId, context.Message.UserId);
        }
    }
}
