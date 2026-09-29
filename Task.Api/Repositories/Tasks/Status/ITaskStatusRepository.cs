using Task.Api.DTO;

namespace Task.Api.Repositories.Tasks.Status
{
    public interface ITaskStatusRepository
    {
        Task<IEnumerable<TaskStatusGet>> GetList();
    }
}
