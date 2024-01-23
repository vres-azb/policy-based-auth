namespace PolicyBased.Infra.Dtos
{
    public class Subject
    {
        public int Id { get; set; } = default!;
        public string UserId { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public bool IsSelected { get; set; } = default;
    }
}
