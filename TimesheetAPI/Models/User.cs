namespace TimesheetAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        // Relationship to limit access
        public List<UserClient> UserClients { get; set; } = new();
        public List<TimesheetEntry> TimesheetEntries { get; set; } = new();
    }

}
