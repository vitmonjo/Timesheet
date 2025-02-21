namespace TimesheetAPI.Models
{
    using System.Text.Json.Serialization;

    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int ClientId { get; set; }  // Foreign Key
    }


}
