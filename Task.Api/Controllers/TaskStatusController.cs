using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Clients.Tasks;
using Task.Api.Repositories.Tasks.Status;
using Task.Api.Services.Tasks;

namespace Task.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskStatusController : AuthroizedController
    {
        private readonly ITaskStatusRepository _taskStatusRepository;

        public TaskStatusController(ITaskStatusRepository taskStatusRepository)
        {
            _taskStatusRepository = taskStatusRepository;
        }

        /// <summary>
        /// Zwraca statusy zadania
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetList()
        {
            var results = await _taskStatusRepository.GetList();
            return Ok(results);
        }
    }
}
