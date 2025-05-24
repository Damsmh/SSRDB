using SSRDB.Entities;

namespace SSRDB.Repositories.Interfaces
{
    public interface IPrescriptionRepository
    {
        Task<IEnumerable<Prescription>> GetAllAsync();
        Task<Prescription> GetByIdAsync(int id);
        Task<IEnumerable<Prescription>> GetByDiagnosisIdAsync(int diagnosisId);
        Task<IEnumerable<Prescription>> GetByMedicationIdAsync(int miagnosisId);
        Task AddAsync(Prescription Prescription);
        Task UpdateAsync(Prescription Prescription);
        Task DeleteAsync(int id);
    }
}
