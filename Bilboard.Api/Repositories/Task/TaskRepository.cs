using Bilboard.Api.DTO.Tasks;
using Bilboard.Api.Resources.Task;
using Common.Attributes.DependencyInjection;
using Common.Services.Database;

namespace Bilboard.Api.Repositories.Task
{
    [DependencyInjection]
    public class TaskRepository : ITaskRepository
    {
        private readonly IDatabaseService _databaseService;

        public TaskRepository(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async System.Threading.Tasks.Task ChangeStatus(TaskChangeStatusDto dto)
        {
            await _databaseService.Update(Sql.Tasks_Task_ChangeStatus, parameters: dto);
        }

        public async System.Threading.Tasks.Task Create(TaskCreateDto dto)
        {
            await _databaseService.Create(Sql.Tasks_Task_Create, parameters: dto);
        }

        public async System.Threading.Tasks.Task Delete(TaskDeleteDto dto)
        {
            await _databaseService.Delete(Sql.Tasks_Task_Delete, parameters: dto);
        }

        public async Task<TaskGetDto?> GetByExternalId(int externalTaskId)
        {
            return await _databaseService.Get<TaskGetDto>(Sql.Tasks_Task_Get, parameters: new { ExternalTaskId = externalTaskId });
        }

        public async System.Threading.Tasks.Task Update(TaskUpdateDto dto)
        {
            await _databaseService.Update(Sql.Tasks_Task_Update, parameters: dto);
        }
    }
}
