using SSRDB.Entities;

namespace SSRDB.Repositories.Interfaces
{
    public interface IServiceRepository
    {
        Task<IEnumerable<Service>> GetAllAsync();
        Task<Service> GetByIdAsync(int id);
        Task<IEnumerable<Service>?> SortByColumn(string column, string method);
        Task AddAsync(Service service);
        Task UpdateAsync(Service service);
        Task DeleteAsync(int id);
    }
}
