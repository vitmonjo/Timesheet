namespace TimesheetAPI.DTOs
{
    public class ProjectDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int ClientId { get; set; }  // Foreign Key
    }
}
