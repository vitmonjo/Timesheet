namespace TimesheetAPI.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public List<Project> Projects { get; set; } = new();
        public List<UserClient> UserClients { get; set; } = new();  // Users assigned
    }

}
