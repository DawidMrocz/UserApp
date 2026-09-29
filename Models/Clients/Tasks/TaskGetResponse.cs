using Models.Enums;
using Newtonsoft.Json;

namespace Models.Clients.Tasks
{
    public class TaskGetResponse
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string Title { get; set; } = null!;
        public DateTime? Deadline { get; set; }
        public bool IsImportant { get; set; }
        public decimal? EstimatedHours { get; set; }
        public string Status { get; set; } = null!;
        public List<TaskFile> Files { get; set; } = [];
        public string FileJson
        {
            set
            {
                Files = JsonConvert.DeserializeObject<List<TaskFile>>(value) ?? [];
            }
        }
    }

    public class TaskFile
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string Name { get; set; } = null!;
    }
}
