using Bilboard.Api.DTO.Bilboard;
using Bilboard.Api.Repositories.Bilboard;
using Common.Attributes.DependencyInjection;
using Models.Clients.Bilboards;
using System.Security.Claims;

namespace Bilboard.Api.Services.Bilboards.Bilboard
{
    [DependencyInjection]
    public class BilboardService : IBilboardService
    {
        private readonly IBilboardRepository _bilboardRepository;

        public BilboardService(IBilboardRepository bilboardRepository)
        {
            _bilboardRepository = bilboardRepository;
        }

        public async Task<BilboardGetResponse> Get(ClaimsPrincipal user)
        {
            BilboardGetItem model = await _bilboardRepository.Get(int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!))
                ?? throw new Exception("Nie znaleziono koszyka");

            return new BilboardGetResponse()
            {
                Id = model.Id,
                FirstName = user.FindFirstValue(ClaimTypes.Name)!,
                LastName = user.FindFirstValue(ClaimTypes.Surname)!,
                BilboardItems = model.BilboardItems
            };
        }
    }
}
