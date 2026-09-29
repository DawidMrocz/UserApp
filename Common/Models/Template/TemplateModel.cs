namespace Common.Models.Template
{
    public class TemplateModel
    {
        public int Id { get; set; }
        public string Body { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string StrongName { get; set; } = null!;
        public string? Description { get; set; }
    }
}
