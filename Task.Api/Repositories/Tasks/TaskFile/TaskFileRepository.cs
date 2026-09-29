using Common.Attributes.DependencyInjection;
using Common.Services.Database;
using Task.Api.Resources.Tasks.TaskFile;
using Task.Api.Services.Tasks;

namespace Task.Api.Repositories.Tasks.TaskFile
{
    [DependencyInjection]
    public class TaskFileRepository : ITaskFileRepository
    {
        private readonly IDatabaseService _databaseService;

        public TaskFileRepository(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<int> Create(CreateTaskFileDto dto)
        {
            return await _databaseService.Create(Sql.Task_TaskFile_Create, parameters: dto);
        }

        public async System.Threading.Tasks.Task Delete(Guid guid, int userId)
        {
            await _databaseService.Delete(Sql.Task_TaskFile_Delete, parameters: new { Guid = guid, UserId = userId });
        }

        public async Task<GetTaskFileDto?> Get(Guid guid)
        {
            return await _databaseService.Get<GetTaskFileDto>(Sql.Task_TaskFile_Get, parameters: new { Guid = guid });
        }

        public async Task<IEnumerable<GetTaskFileDto>> GetListForTask(int taskId)
        {
            return await _databaseService.GetList<GetTaskFileDto>(Sql.Task_TaskFile_GetList, parameters: new { TaskId = taskId });
        }
    }
}
