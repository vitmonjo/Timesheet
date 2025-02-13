namespace TimesheetAPI.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int ClientId { get; set; }  // Foreign Key

        // Navigation properties
        public Client Client { get; set; } = null!;
        public List<Task> Tasks { get; set; } = new();
    }

}
