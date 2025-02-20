namespace TimesheetAPI.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class UserClient
    {
        public int Id { get; set; }

        [Key]
        [Column(Order = 1)]
        public int UserId { get; set; }

        [Key]
        [Column(Order = 2)]
        public int ClientId { get; set; }

        // Navigation properties
        public User User { get; set; }
        public Client Client { get; set; }
    }
}
