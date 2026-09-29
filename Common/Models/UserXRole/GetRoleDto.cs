namespace Common.Models.UserXRole
{
    public class GetRoleDto
    {
        public int RoleId { get; set; }
        public string Name { get; set; } = null!;
        public string StrongName { get; set; } = null!;
    }
}
