namespace Models.Clients.Tasks
{
    public class TaskFileGetResponse
    {
        public string FileName { get; set; } = null!;
        public string MimeType { get; set; } = null!;
        public byte[] Content { get; set; } = null!;
    }
}