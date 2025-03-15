using backendclinic.Models;
using backendclinic.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using backendclinic.Services;

namespace backendclinic.Controllers
{
    [Route("api/appointments")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly GreenApiService _greenApiService;
        private readonly IUserRepository _userRepository;

        public AppointmentController(IAppointmentRepository appointmentRepository, GreenApiService greenApiService, IUserRepository userRepository)
        {
            _appointmentRepository = appointmentRepository;
            _greenApiService = greenApiService;
            _userRepository = userRepository;
        }

        [HttpPost("book")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> BookAppointment([FromBody] AppointmentRequest request)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            if (request.AppointmentDateTime == default)
            {
                return BadRequest(new { success = false, message = "Invalid appointment date." });
            }

            if (request.AppointmentDateTime <= DateTime.Now)
            {
                return BadRequest(new { success = false, message = "The appointment date must be in the future." });
            }

            var existingAppointment = await _appointmentRepository.GetAppointmentsAsync();
            var conflictingAppointment = existingAppointment.Any(a =>
                (a.AppointmentDateTime >= request.AppointmentDateTime.AddHours(-1) &&
                 a.AppointmentDateTime <= request.AppointmentDateTime.AddHours(1)) &&
                a.Status != 0);

            if (conflictingAppointment)
            {
                return Conflict(new { success = false, message = "There is already an appointment within 1 hour of the requested time." });
            }

            var appointment = new Appointment
            {
                UserId = userId,
                AppointmentDateTime = request.AppointmentDateTime,
                Status = 0
            };

            try
            {
                await _appointmentRepository.BookAppointmentAsync(appointment);

                // ✅ Get user phone number
                var user = await _userRepository.GetUserByIdAsync(userId);
                if (user != null && !string.IsNullOrEmpty(user.PhoneNumber))
                {
                    string message = $"Hello {user.FirstName}, your appointment on {request.AppointmentDateTime} is waiting for admin approval.";
                    await _greenApiService.SendWhatsAppMessageAsync(user.PhoneNumber, message);
                }

                return Ok(new { success = true, message = "Appointment booked successfully. A WhatsApp notification has been sent." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An error occurred while booking the appointment.", error = ex.Message });
            }
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAppointments()
        {
            try
            {
                var appointments = await _appointmentRepository.GetAppointmentsAsync();
                var appointmentDTOs = appointments.Select(a => new AppointmentDTO
                {
                    Id = a.Id,
                    UserId = a.UserId,
                    AppointmentDateTime = a.AppointmentDateTime,
                    Status = a.Status,
                    User = new UserDTO
                    {
                        Id = a.User.Id,
                        FirstName = a.User.FirstName,
                        LastName = a.User.LastName,
                        Email = a.User.Email,
                        Phone_number = a.User.PhoneNumber,
                        Gender = a.User.Gender,
                    }
                }).ToList();

                return Ok(appointmentDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An error occurred while retrieving appointments.", error = ex.Message });
            }
        }

        [HttpPut("update-status/{appointmentId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAppointmentStatus(int appointmentId, [FromBody] AppointmentStatusRequest request)
        {
            try
            {
                var appointment = await _appointmentRepository.GetAppointmentByIdAsync(appointmentId);
                if (appointment == null)
                {
                    return NotFound(new { success = false, message = "Appointment not found." });
                }

                var success = await _appointmentRepository.UpdateAppointmentStatusAsync(appointmentId, request.Status);
                if (!success)
                {
                    return StatusCode(500, new { success = false, message = "Failed to update appointment status." });
                }

                if (request.Status == 1)
                {
                    var user = await _userRepository.GetUserByIdAsync(appointment.UserId);
                    if (user != null && !string.IsNullOrEmpty(user.PhoneNumber))
                    {
                        string message = $"Hello {user.FirstName}, your appointment on {appointment.AppointmentDateTime} has been approved. See you soon!";
                        await _greenApiService.SendWhatsAppMessageAsync(user.PhoneNumber, message);
                    }
                }

                return Ok(new { success = true, message = "Appointment status updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An error occurred while updating the appointment status.", error = ex.Message });
            }
        }

    }

    public class AppointmentRequest
    {
        public DateTime AppointmentDateTime { get; set; }
    }

    public class AppointmentStatusRequest
    {
        public int Status { get; set; }
    }
}
