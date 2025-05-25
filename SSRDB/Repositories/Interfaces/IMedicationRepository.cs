using SSRDB.Entities;

namespace SSRDB.Repositories.Interfaces
{
    public interface IMedicationRepository
    {
        Task<IEnumerable<Medication>> GetAllAsync();
        Task<Medication> GetByIdAsync(int id);
        Task<IEnumerable<Medication>?> SortByColumn(string column, string method);
        Task AddAsync(Medication medication);
        Task UpdateAsync(Medication medication);
        Task DeleteAsync(int id);
    }
}
