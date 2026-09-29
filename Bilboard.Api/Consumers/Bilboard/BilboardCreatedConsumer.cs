using Bilboard.Api.Repositories.Bilboard;
using MassTransit;
using Models.Rabbit.Bilboard;

namespace Bilboard.Api.Consumers.Task
{
    public class BilboardCreatedConsumer : IConsumer<CreateBilboardItemEvent>
    {
        private readonly IBilboardRepository _bilboardRepository;

        public BilboardCreatedConsumer(IBilboardRepository bilboardRepository)
        {
            _bilboardRepository = bilboardRepository;
        }

        public async System.Threading.Tasks.Task Consume(ConsumeContext<CreateBilboardItemEvent> context)
        {
            await _bilboardRepository.Create(context.Message.UserId);
        }
    }
}
