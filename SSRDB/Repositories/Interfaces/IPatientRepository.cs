using SSRDB.Entities;

namespace SSRDB.Repositories.Interfaces
{
    public interface IPatientRepository
    {
        Task<IEnumerable<Patient>> GetAllAsync();
        Task<Patient> GetByIdAsync(int id);
        Task<IEnumerable<Patient>?> SortByColumn(string column, string method);
        Task AddAsync(Patient patient);
        Task UpdateAsync(Patient patient);
        Task DeleteAsync(int id);
    }
}
