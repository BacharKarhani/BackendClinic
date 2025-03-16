namespace backendclinic.Models
{
    public class AppointmentDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public int Status { get; set; }
        public UserDTO User { get; set; }
        public string? DeclineReason { get; set; } // ✅ Allow NULL values

    }
}
