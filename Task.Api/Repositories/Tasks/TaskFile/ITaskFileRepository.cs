using Task.Api.Services.Tasks;

namespace Task.Api.Repositories.Tasks.TaskFile
{
    public interface ITaskFileRepository
    {
        Task<int> Create(CreateTaskFileDto dro);
        Task<IEnumerable<GetTaskFileDto>> GetListForTask(int taskId);
        Task<GetTaskFileDto?> Get(Guid guid);
        System.Threading.Tasks.Task Delete(Guid guid, int userId);
    }
}
