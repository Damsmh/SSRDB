using SSRDB.Entities;

namespace SSRDB.Repositories.Interfaces
{
    public interface IDiagnosisRepository
    {
        Task<IEnumerable<Diagnosis>> GetAllAsync();
        Task<Diagnosis> GetByIdAsync(int id);
        Task<IEnumerable<Diagnosis>> GetByAppointmentIdAsync(int appointmentId);
        Task AddAsync(Diagnosis Diagnosis);
        Task UpdateAsync(Diagnosis Diagnosis);
        Task DeleteAsync(int id);
    }
}
