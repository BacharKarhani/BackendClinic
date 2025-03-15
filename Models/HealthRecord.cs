namespace backendclinic.Models
{
    public class HealthRecord
    {
        public int Id { get; set; }
        public int UserId { get; set; } // Foreign Key to User
        public string Condition { get; set; }
        public string Treatment { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }

    }
}
