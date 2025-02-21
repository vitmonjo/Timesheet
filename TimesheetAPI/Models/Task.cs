namespace TimesheetAPI.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int ProjectId { get; set; }  // Foreign Key
    }
}
