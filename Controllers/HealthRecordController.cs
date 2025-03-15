using backendclinic.Models;
using backendclinic.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace backendclinic.Controllers
{
    [Route("api/healthrecords")]
    [ApiController]
    public class HealthRecordController : ControllerBase
    {
        private readonly IHealthRecordRepository _healthRecordRepository;

        public HealthRecordController(IHealthRecordRepository healthRecordRepository)
        {
            _healthRecordRepository = healthRecordRepository;
        }

        // Admin can add a health record
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddHealthRecord([FromBody] HealthRecord record)
        {
            try
            {
                // Ensure UserId is provided
                if (record.UserId <= 0)
                {
                    return BadRequest(new { success = false, message = "UserId is required." });
                }

                // Ensure required fields are present
                if (string.IsNullOrEmpty(record.Condition) ||
                    string.IsNullOrEmpty(record.Treatment) ||
                    record.Date == default ||
                    string.IsNullOrEmpty(record.Notes))
                {
                    return BadRequest(new { success = false, message = "All fields (Condition, Treatment, Date, Notes) are required." });
                }

                // Create a new health record object
                var newRecord = new HealthRecord
                {
                    UserId = record.UserId,
                    Condition = record.Condition,
                    Treatment = record.Treatment,
                    Date = record.Date,
                    Notes = record.Notes
                };

                // Save to database
                await _healthRecordRepository.AddHealthRecordAsync(newRecord);

                return Ok(new { success = true, message = "Health record added successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An error occurred while adding the health record.", error = ex.Message });
            }
        }

        // Admin can update a health record
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateHealthRecord(int id, [FromBody] HealthRecord record)
        {
            try
            {
                var existingRecord = await _healthRecordRepository.GetHealthRecordAsync(id);
                if (existingRecord == null)
                {
                    return NotFound(new { success = false, message = "Health record not found." });
                }

                existingRecord.Condition = record.Condition;
                existingRecord.Treatment = record.Treatment;
                existingRecord.Date = record.Date;
                existingRecord.Notes = record.Notes;

                await _healthRecordRepository.UpdateHealthRecordAsync(existingRecord);
                return Ok(new { success = true, message = "Health record updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An error occurred while updating the health record.", error = ex.Message });
            }
        }

        // Admin can delete a health record
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteHealthRecord(int id)
        {
            try
            {
                var existingRecord = await _healthRecordRepository.GetHealthRecordAsync(id);
                if (existingRecord == null)
                {
                    return NotFound(new { success = false, message = "Health record not found." });
                }

                await _healthRecordRepository.DeleteHealthRecordAsync(id);
                return Ok(new { success = true, message = "Health record deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An error occurred while deleting the health record.", error = ex.Message });
            }
        }

        // Get all health records for a specific user (admin only)
        [HttpGet("user/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetHealthRecordsByUserId(int userId)
        {
            try
            {
                var records = await _healthRecordRepository.GetHealthRecordsByUserIdAsync(userId);
                return Ok(records);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An error occurred while retrieving health records.", error = ex.Message });
            }
        }

        // Get the logged-in user's health records
        [HttpGet("me")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetHealthRecordsForLoggedInUser()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var records = await _healthRecordRepository.GetHealthRecordsByUserIdAsync(userId);

                if (records == null || !records.Any())
                {
                    return Ok(new { success = true, message = "No health records found for this user." });
                }

                return Ok(records);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An error occurred while retrieving health records.", error = ex.Message });
            }
        }
    }
}
