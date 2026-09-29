using Common.ApiModels;
using Models.Enums;

namespace Models.Clients.Tasks
{
    public class TaskSearchResponse: SearchItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public DateTime? Deadline { get; set; }
        public bool IsImportant { get; set; }
        public string Status { get; set; } = null!;
    }
}
