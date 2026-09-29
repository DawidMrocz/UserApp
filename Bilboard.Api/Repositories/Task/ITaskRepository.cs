using Bilboard.Api.DTO.Tasks;

namespace Bilboard.Api.Repositories.Task
{
    public interface ITaskRepository
    {
        System.Threading.Tasks.Task Create(TaskCreateDto request);
        System.Threading.Tasks.Task Delete(TaskDeleteDto request);
        Task<TaskGetDto?> GetByExternalId(int externalTaskId);
        System.Threading.Tasks.Task Update(TaskUpdateDto request);
        System.Threading.Tasks.Task ChangeStatus(TaskChangeStatusDto request);
    }
}
