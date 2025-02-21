namespace TimesheetAPI.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class UserClient
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ClientId { get; set; }
    }
}
