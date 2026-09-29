using Common.ApiModels;
using Common.Attributes.DependencyInjection;
using Common.Helpers;
using Common.Services.File;
using MassTransit;
using Models.Clients.Tasks;
using Models.Enums;
using Models.Rabbit.Task;
using Task.Api.DTO;
using Task.Api.Repositories.Tasks.Status;
using Task.Api.Repositories.Tasks.Task;
using Task.Api.Repositories.Tasks.TaskFile;

namespace Task.Api.Services.Tasks
{
    public class CreateTaskFileDto
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public int TaskId { get; set; }
        public string FileName { get; set; } = null!;
        public int? UserId { get; set; }
        public string? RoleStrongName { get; set; }
    }

    public class GetTaskFileDto : CreateTaskFileDto
    {
        public int Id { get; set; }
    }

    [DependencyInjection]
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IFileService _fileService;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ITaskFileRepository _taskFileRepository;
        private readonly ITaskStatusRepository _taskStatusRepository;

        public TaskService(ITaskRepository taskRepository, IFileService fileService, IPublishEndpoint publishEndpoint, ITaskFileRepository taskFileRepository, ITaskStatusRepository taskStatusRepository)
        {
            _taskRepository = taskRepository;
            _fileService = fileService;
            _publishEndpoint = publishEndpoint;
            _taskFileRepository = taskFileRepository;
            _taskStatusRepository = taskStatusRepository;
        }

        public async System.Threading.Tasks.Task AddToBilboard(int taskId, int userId)
        {
            await _taskRepository.AddToBilboard(taskId, userId);

            await _publishEndpoint.Publish(new TaskAddedToBilboardEvent()
            {
                TaskId = taskId,
                UserId = userId
            });
        }

        public async Task<int> Create(TaskCreateRequest request, int userId)
        {
            CreateTaskDto dto = new()
            {
                Title = request.Title,
                Deadline = request.Deadline,
                IsImportant = request.IsImportant,
                EstimatedHours = request.EstimatedHours,
                UserId = userId,
                StatusId = 1
            };

            int taskId = await _taskRepository.Create(dto);

            await _publishEndpoint.Publish(new TaskCreatedEvent()
            {
                TaskId = taskId,
                Title = dto.Title,
                StatusExternalId = (int)TaskStatusEnum.Backlog,
                Deadline = dto.Deadline,
                IsImportant = dto.IsImportant,
                EstimatedHours = dto.EstimatedHours,
                UserId = userId
            });

            return taskId;
        }

        public async System.Threading.Tasks.Task CreateDocument(int taskId, string fileName, byte[] content, int userId)
        {
            Guid fileGuid = Guid.NewGuid();

            CreateTaskFileDto fileDto = new()
            {
                Guid = fileGuid,
                FileName = fileName,
                UserId = userId,
                TaskId = taskId
            };

            await _taskFileRepository.Create(fileDto);

            await _fileService.Create(content, fileName, fileGuid);
        }

        public async System.Threading.Tasks.Task Delete(int taskId, int userId)
        {
            using var transaction = TransactionHelper.GetTransactionScope();

            await _taskRepository.Delete(taskId, userId);

            await _publishEndpoint.Publish(new TaskDeletedEvent()
            {
                TaskId = taskId,
                UserId = userId
            });
        }

        public async System.Threading.Tasks.Task DeleteDocument(Guid documentGuid, int userId)
        {
            GetTaskFileDto file = await _taskFileRepository.Get(documentGuid)
                ?? throw new Exception("FIle not found");

            await _fileService.GetFilesFromDisc(file.FileName, documentGuid);

            await _taskFileRepository.Delete(documentGuid, userId);
        }

        public async Task<TaskGetResponse> Get(int taskId)
        {
            return await _taskRepository.Get(taskId)
                ?? throw new Exception($"Unable to get {taskId}");
        }

        public async Task<(byte[] fileContent, string mimeType, string fileDownloadName)> GetDocument(int taskId, Guid documentGuid, int userId)
        {
            List<GetTaskFileDto> files = (await _taskFileRepository.GetListForTask(taskId)).ToList();

            GetTaskFileDto fileToDelete = files.FirstOrDefault(f => f.Guid == documentGuid)
                ?? throw new Exception("File not found");

            return await _fileService.GetFilesFromDisc(fileToDelete.FileName, fileToDelete.Guid);
        }

        public async Task<SearchResponse<TaskSearchResponse>> Search(TaskSearchRequest request)
        {
            var results = await _taskRepository.Search(request);
            return new SearchResponse<TaskSearchResponse>(results);
        }

        public async Task<int> Update(int taskId, TaskUpdateRequest request, int userId)
        {
            await _taskRepository.Update(taskId, request, userId);

            await _publishEndpoint.Publish(new TaskUpdatedEvent()
            {
                TaskId = taskId,
                Title = request.Title,
                Deadline = request.Deadline,
                IsImportant = request.IsImportant,
                EstimatedHours = request.EstimatedHours,
                UserId = userId
            });

            return taskId;
        }

        public async Task<TaskFileGetResponse> GetDocument(Guid guid)
        {
            GetTaskFileDto file = await _taskFileRepository.Get(guid)
                ?? throw new Exception("FIle not found");

            (byte[] fileContent, string mimeType, string fileName) = await _fileService.GetFilesFromDisc(file.FileName, guid);

            return new TaskFileGetResponse()
            {
                FileName = fileName,
                Content = fileContent,
                MimeType = mimeType,
            };
        }
    }
}
