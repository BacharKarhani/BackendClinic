using backendclinic.Models;

namespace backendclinic.Repositories
{
    public interface IHealthRecordRepository
    {
        Task AddHealthRecordAsync(HealthRecord record);
        Task UpdateHealthRecordAsync(HealthRecord record);
        Task DeleteHealthRecordAsync(int id);
        Task<HealthRecord> GetHealthRecordAsync(int id);
        Task<IEnumerable<HealthRecord>> GetHealthRecordsByUserIdAsync(int userId);
    }
}
