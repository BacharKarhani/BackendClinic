using backendclinic.Data;
using backendclinic.Models;
using Microsoft.EntityFrameworkCore;

namespace backendclinic.Repositories
{
    public class HealthRecordRepository : IHealthRecordRepository
    {
        private readonly ApplicationDbContext _context;

        public HealthRecordRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddHealthRecordAsync(HealthRecord record)
        {
            _context.HealthRecords.Add(record);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateHealthRecordAsync(HealthRecord record)
        {
            _context.HealthRecords.Update(record);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteHealthRecordAsync(int id)
        {
            var record = await _context.HealthRecords.FindAsync(id);
            if (record != null)
            {
                _context.HealthRecords.Remove(record);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<HealthRecord> GetHealthRecordAsync(int id)
        {
            return await _context.HealthRecords.FindAsync(id);
        }

        public async Task<IEnumerable<HealthRecord>> GetHealthRecordsByUserIdAsync(int userId)
        {
            return await _context.HealthRecords
                                 .Where(r => r.UserId == userId)
                                 .ToListAsync();
        }
    }
}
