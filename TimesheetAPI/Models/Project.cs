namespace TimesheetAPI.Models
{
    using System.Text.Json.Serialization;

    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int ClientId { get; set; }  // Foreign Key

        [JsonIgnore]  // 🔹 Ignore Client in incoming requests
        public Client? Client { get; set; }

        public List<Task> Tasks { get; set; } = new();
    }


}
