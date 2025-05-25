using SSRDB.Entities;

namespace SSRDB.Repositories.Interfaces
{
    public interface IAppointmentServiceRepository
    {
        Task<AppointmentService> GetByIdAsync(int id);
        Task<IEnumerable<AppointmentService>> GetAllAsync();
        Task<IEnumerable<AppointmentService>> GetByAppointmentIdAsync(int appointmentId);
        Task<IEnumerable<AppointmentService>> GetByServiceIdAsync(int serviceId);
        Task<IEnumerable<AppointmentService>?> SortByColumn(string column, string method);
        Task AddAsync(AppointmentService appointmentService);
        Task UpdateAsync(AppointmentService appointmentService);
        Task DeleteAsync(int id);
    }
}
