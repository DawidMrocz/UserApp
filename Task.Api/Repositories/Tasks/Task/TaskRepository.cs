using Common.Attributes.DependencyInjection;
using Common.Services.Database;
using Models.Clients.Tasks;
using Task.Api.DTO;
using Task.Api.Resources.Tasks.Task;

namespace Task.Api.Repositories.Tasks.Task
{
    [DependencyInjection]
    public class TaskRepository : ITaskRepository
    {
        private readonly IDatabaseService _databaseService;

        public TaskRepository(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async System.Threading.Tasks.Task AddToBilboard(int taskId, int userId)
        {
            await _databaseService.Update(Sql.Tasks_Task_AddToBilboard, parameters: new { TaskId = taskId, UserId = userId });
        }

        public async System.Threading.Tasks.Task ChangeStatus(int taskId, string statusStrongName, int userId)
        {
            await _databaseService.Update(Sql.Tasks_Task_ChangeStatus, parameters: new { TaskId = taskId, StatusStrongName = statusStrongName, UserId = userId });
        }

        public async Task<int> Create(CreateTaskDto dto)
        {
            return await _databaseService.GetValue<int>(Sql.Tasks_Task_Create, parameters: dto);
        }

        public async System.Threading.Tasks.Task Delete(int taskId, int userId)
        {
            await _databaseService.Delete(Sql.Tasks_Task_Delete, parameters: new { TaskId = taskId, UserId = userId });
        }

        public async Task<TaskGetResponse?> Get(int taskId)
        {
            return await _databaseService.Get<TaskGetResponse>(Sql.Tasks_Task_Get, parameters: new { TaskId = taskId });
        }

        public async System.Threading.Tasks.Task RemoveUser(int taskId, int userId)
        {
            await _databaseService.Update(Sql.Tasks_Task_RemoveUser, parameters: new { TaskId = taskId, UserId = userId });
        }

        public async Task<IEnumerable<TaskSearchResponse>> Search(TaskSearchRequest request)
        {
            return await _databaseService.GetList<TaskSearchResponse>(Sql.Tasks_Task_Search, parameters: request);
        }

        public async Task<int> Update(int taskId, TaskUpdateRequest request, int userId)
        {
            return await _databaseService.Update(Sql.Tasks_Task_Update, parameters: [request, new { TaskId = taskId, UserId = userId }]);
        }
    }
}
