using Common.ApiModels;
using Models.Clients.Tasks;

namespace Task.Api.Services.Tasks
{
    public interface ITaskService
    {
        Task<int> Create(TaskCreateRequest request, int userId);
        System.Threading.Tasks.Task Delete(int taskId, int userId);
        Task<TaskGetResponse> Get(int taskId);
        Task<SearchResponse<TaskSearchResponse>> Search(TaskSearchRequest request);
        Task<int> Update(int taskId, TaskUpdateRequest request, int userId);
        System.Threading.Tasks.Task AddToBilboard(int taskId, int userId);
        Task<TaskFileGetResponse> GetDocument(Guid documentGuid);
        System.Threading.Tasks.Task CreateDocument(int taskId, string fileName, byte[] content, int userId);
        System.Threading.Tasks.Task DeleteDocument(Guid documentGuid, int userId);
    }
}
