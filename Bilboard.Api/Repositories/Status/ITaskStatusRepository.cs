using Models.Clients.Tasks;

namespace Bilboard.Api.Repositories.Status
{
    public interface ITaskStatusRepository
    {
        Task<IEnumerable<TaskStatusGet>> GetList();
    }
}
