namespace TimesheetAPI.Models
{
    public class TimesheetEntry
    {
        public int Id { get; set; }
        public int UserId { get; set; }  // Foreign Key
        public int TaskId { get; set; }  // Foreign Key
        public DateTime Date { get; set; }  // Work date
        public decimal HoursWorked { get; set; }  // Hours worked
        public string Description { get; set; } = string.Empty;  // Optional notes
    }

}
