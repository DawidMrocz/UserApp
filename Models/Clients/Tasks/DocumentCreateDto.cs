namespace Models.Clients.Tasks
{
    public class DocumentCreateDto
    {
        public string FileName { get; set; } = null!;
        public byte[] Content { get; set; } = null!;
    }
}
