using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Clients.Tasks;
using Models.Enums;
using Task.Api.Services.Tasks;

namespace Task.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TasksController : AuthroizedController
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        /// <summary>
        /// Wyszukiwarka zadañ
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Wyszukiwarka zadañ</returns>
        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] TaskSearchRequest request)
        {
            return Ok(await _taskService.Search(request));
        }

        /// <summary>
        /// Widok zadania
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns>Widok zadania</returns>
        [HttpGet("{taskId}")]
        public async Task<IActionResult> Get([FromRoute] int taskId)
        {
            return Ok(await _taskService.Get(taskId));
        }

        /// <summary>
        /// Widok zadania
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns>Widok zadania</returns>
        [HttpGet("document/{documentGuid}")]
        public async Task<IActionResult> GetDocument([FromRoute] Guid documentGuid)
        {
            return Ok(await _taskService.GetDocument(documentGuid));
        }

        /// <summary>
        /// Widok zadania
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns>Widok zadania</returns>
        [HttpPost("{taskId}/document")]
        [Authorize(Roles = nameof(PermissionEnum.Admin))]
        public async Task<IActionResult> CreateDocument([FromRoute] int taskId, [FromBody] DocumentCreateDto request)
        {
            await _taskService.CreateDocument(taskId, request.FileName, request.Content, AuthorizedUserId!.Value);
            return Ok("Pomyœlnie stworzono dokument");
        }

        /// <summary>
        /// Widok zadania
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns>Widok zadania</returns>
        [HttpDelete("document/{documentGuid}")]
        [Authorize(Roles = nameof(PermissionEnum.Admin))]
        public async Task<IActionResult> DeleteDocument([FromRoute] Guid documentGuid)
        {
            await _taskService.DeleteDocument(documentGuid, AuthorizedUserId!.Value);
            return Ok("Pomyœlnie usuniêto dokument");
        }

        /// <summary>
        /// Tworzenie zadania
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Tworzenie zadania</returns>
        [HttpPost]
        [Authorize(Roles = nameof(PermissionEnum.Admin))]
        public async Task<IActionResult> Create([FromBody] TaskCreateRequest request)
        {
            return Ok(await _taskService.Create(request, AuthorizedUserId!.Value));
        }

        /// <summary>
        /// Aktualizacja zadania
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="request"></param>
        /// <returns>Aktualizacja zadania</returns>
        [HttpPut("{taskId}")]
        [Authorize(Roles = nameof(PermissionEnum.Admin))]
        public async Task<IActionResult> Update([FromRoute] int taskId, [FromBody] TaskUpdateRequest request)
        {
            return Ok(await _taskService.Update(taskId, request, AuthorizedUserId!.Value));
        }

        /// <summary>
        /// Usuniêcie zadania
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns>Usuniêcie zadania</returns>
        [HttpDelete("{taskId}")]
        [Authorize(Roles = nameof(PermissionEnum.Admin))]
        public async Task<IActionResult> Delete([FromRoute] int taskId)
        {
            await _taskService.Delete(taskId, AuthorizedUserId!.Value);

            return Ok("Pomyœlnie usuniêto zadanie");
        }

        /// <summary>
        /// Dodanie zadania do tablicy
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns>Dodanie zadania do tablicy</returns>
        [HttpPut("{taskId}/add-to-bilboard")]
        public async Task<IActionResult> AddToBilboard([FromRoute] int taskId)
        {
            await _taskService.AddToBilboard(taskId, AuthorizedUserId!.Value);

            return Ok("Pomyœlnie dodano zadanie do tablicy");
        }
    }
}
