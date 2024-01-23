namespace PolicyBased.Infra.Dtos
{
    public class Role
    {
        public string Name { get; set; } = default!;
        public int PolicyId { get; set; } = default!;
        public int RoleId { get; set; } = default!;
        public bool IsSelected { get; set; } = default!;
        public List<Permission> Permissions { get; set; } = new();
        public List<Subject> Subjects { get; set; } = new();
        public List<string> IdentityRoles { get; set; } = new();

    }
}