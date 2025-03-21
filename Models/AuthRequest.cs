﻿using System.ComponentModel.DataAnnotations;

namespace backendclinic.Models
{
    public class RegisterRequest
    {
        [Required] public string FirstName { get; set; }
        [Required] public string LastName { get; set; }
        [Required] public string Email { get; set; }
        [Required] public string Password { get; set; }
        [Required] public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Role { get; set; } = "User";
    }

    public class LoginRequest
    {
        [Required] public string Email { get; set; }
        [Required] public string Password { get; set; }
    }
}
