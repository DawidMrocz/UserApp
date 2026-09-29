using Common.ApiModels;
using Common.Extensions;
using Gateway.Api.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Clients.Tasks;
using Models.Enums;

namespace Gateway.Api.Controllers.Tasks
{
    [ApiController]
    [Route("[controller]")]
    public class TasksController : GatewayController
    {
        private readonly ITokenService _tokenService;
        private readonly HttpClient _taskClient;

        public TasksController(IHttpClientFactory httpClientFactory, ITokenService tokenService)
        {
            _taskClient = httpClientFactory.CreateClient("TaskApi");
            _tokenService = tokenService;
        }

        /// <summary>
        /// Wyszukiwarka zadañ
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Wyszukiwarka zadañ</returns>
        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] TaskSearchRequest request)
        {
            var results = await _taskClient.Get<SearchResponse<TaskSearchResponse>, TaskSearchRequest>("/Tasks", request, _tokenService.GetToken());
            return Ok(results);
        }

        /// <summary>
        /// Widok zadania
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns>Widok zadania</returns>
        [HttpGet("{taskId}")]
        public async Task<IActionResult> Get([FromRoute] int taskId)
        {
            TaskGetResponse result = await _taskClient.Get<TaskGetResponse>($"/Tasks/{taskId}", _tokenService.GetToken());
            return Ok(result);
        }

        /// <summary>
        /// Stworzenie zadania
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Stworzenie zadania</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TaskCreateRequest request)
        {
            var result = await _taskClient.Create<int, TaskCreateRequest>($"/Tasks/", request, _tokenService.GetToken());
            return Ok(result);
        }

        /// <summary>
        /// Aktualziacja zadania
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="request"></param>
        /// <returns>Aktualziacja zadania</returns>
        [HttpPut("{taskId}")]
        public async Task<IActionResult> Update([FromRoute] int taskId, [FromBody] TaskUpdateRequest request)
        {
            var result = await _taskClient.Update<int, TaskUpdateRequest>($"/Tasks/{taskId}", request, _tokenService.GetToken());
            return Ok(result);
        }

        /// <summary>
        /// Usuniecie zadania
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns>Usuniecie zadania</returns>
        [HttpDelete("{taskId}")]
        public async Task<IActionResult> Delete([FromRoute] int taskId)
        {
            await _taskClient.Delete($"/Tasks/{taskId}", _tokenService.GetToken());
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
            await _taskClient.Update($"/Tasks/{taskId}/add-to-bilboard", _tokenService.GetToken());

            return Ok("Pomyœlnie dodano zadanie do tablicy");
        }

        /// <summary>
        /// Widok zadania
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns>Widok zadania</returns>
        [HttpGet("document/{documentGuid}")]
        public async Task<IActionResult> GetDocument([FromRoute] Guid documentGuid)
        {
            TaskFileGetResponse response = await _taskClient.Get<TaskFileGetResponse>($"/Tasks/document/{documentGuid}", _tokenService.GetToken());
            return File(response.Content, response.MimeType, response.FileName);
        }

        /// <summary>
        /// Widok zadania
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns>Widok zadania</returns>
        [HttpPost("{taskId}/document")]
        public async Task<IActionResult> CreateDocument([FromRoute] int taskId, [FromForm] IFormFile file)
        {
            DocumentCreateDto dto = new()
            {
                FileName = file.FileName,
                Content = await file.ConvertFileToByteArrayAsync()
            };
            await _taskClient.Create($"/Tasks/{taskId}/document", dto, _tokenService.GetToken());
            return Ok("Pomyœlnie stworzono dokument");
        }

        /// <summary>
        /// Widok zadania
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns>Widok zadania</returns>
        [HttpDelete("document/{documentGuid}")]
        public async Task<IActionResult> DeleteDocument([FromRoute] string documentGuid)
        {
            await _taskClient.Delete($"/Tasks/document/{documentGuid}", _tokenService.GetToken());
            return Ok("Pomyœlnie usuniêto dokument");
        }

        /// <summary>
        /// Dodanie zadania do tablicy
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns>Dodanie zadania do tablicy</returns>
        [HttpGet("statuses")]
        public async Task<IActionResult> GetStatuses()
        {
            throw new Exception("Wyjebkaaa");
            return Ok(await _taskClient.Get<List<TaskStatusGet>>($"/TaskStatus"));
        }
    }
}
