using SSRDB.Entities;

namespace SSRDB.Repositories.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee> GetByIdAsync(int id);
        Task<Employee> GetByNameAsync(string name);
        Task<IEnumerable<Employee>?> SortByColumn(string column, string method);
        Task AddAsync(Employee employee);
        Task UpdateAsync(Employee employee);
        Task DeleteAsync(int id);
    }
}
