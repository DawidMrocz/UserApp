namespace Common.Models.File
{
    public class FileModel
    {
        public int FileId { get; set; }
        public string Name { get; set; } = null!;
        public Guid Guid { get; set; }
        public int? RoleId { get; set; }
        public int? UserId { get; set; }
        public byte[]? Content { get; set; }
    }
}
