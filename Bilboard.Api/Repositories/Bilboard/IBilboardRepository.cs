using Bilboard.Api.DTO.Bilboard;

namespace Bilboard.Api.Repositories.Bilboard
{
    public interface IBilboardRepository
    {
        Task<BilboardGetItem?> Get(int userId);
        System.Threading.Tasks.Task<int> Create(int userId);
    }
}
