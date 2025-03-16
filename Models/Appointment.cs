using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backendclinic.Models
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [Required]
        public DateTime AppointmentDateTime { get; set; }
        public int Status { get; set; }

        public string? DeclineReason { get; set; } // ✅ Allow NULL values
    }
}
