using Models.Clients.Tasks;
using Task.Api.DTO;

namespace Task.Api.Repositories.Tasks.Task
{
    public interface ITaskRepository
    {
        Task<int> Create(CreateTaskDto dto);
        System.Threading.Tasks.Task Delete(int taskId, int userId);
        Task<TaskGetResponse?> Get(int taskId);
        Task<IEnumerable<TaskSearchResponse>> Search(TaskSearchRequest request);
        Task<int> Update(int taskId, TaskUpdateRequest request, int userId);
        System.Threading.Tasks.Task AddToBilboard(int taskId, int userId);
        System.Threading.Tasks.Task RemoveUser(int taskId, int userId);
        System.Threading.Tasks.Task ChangeStatus(int taskId, string StatusStrongName, int userId);

    }
}
