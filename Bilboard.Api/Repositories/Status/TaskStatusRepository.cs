using Bilboard.Api.Resources.TaskStatus;
using Common.Attributes.DependencyInjection;
using Common.Services.Database;
using Models.Clients.Tasks;

namespace Bilboard.Api.Repositories.Status
{
    [DependencyInjection]
    public class TaskStatusRepository : ITaskStatusRepository
    {
        private readonly IDatabaseService _databaseService;

        public TaskStatusRepository(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<IEnumerable<TaskStatusGet>> GetList()
        {
            return await _databaseService.GetList<TaskStatusGet>(Sql.Task_TaskStatus_GetList);
        }
    }
}
