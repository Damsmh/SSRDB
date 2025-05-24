using SSRDB.Entities;
using SSRDB.Utils;
using SSRDB.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using SSRDB.Repositories.Interfaces;

namespace SSRDB.Repositories
{
    public class AppointmentServiceRepository(ApplicationDbContext context) : IAppointmentServiceRepository
    {
        public async Task<IEnumerable<AppointmentService>> GetAllAsync()
        {
            return await context.AppointmentServices
                .FromSqlRaw($"""SELECT * FROM "AppointmentServices" """)
                .ToListAsync();
        }

        public async Task<AppointmentService> GetByIdAsync(int id)
        {
            var AppointmentServiceId = new NpgsqlParameter("AppointmentServiceId", id);
            return await context.AppointmentServices
                .FromSqlRaw($"""SELECT * FROM "AppointmentServices" WHERE "AppointmentServiceId" = @AppointmentServiceId""", AppointmentServiceId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<AppointmentService>> GetByAppointmentIdAsync(int id)
        {
            var AppointmentId = new NpgsqlParameter("AppointmentId", id);
            return await context.AppointmentServices
                .FromSqlRaw($"""SELECT * FROM "AppointmentServices" WHERE "AppointmentId" = @AppointmentId""", AppointmentId)
                .ToListAsync();
        }

        public async Task<IEnumerable<AppointmentService>> GetByServiceIdAsync(int id)
        {
            var ServiceId = new NpgsqlParameter("ServiceId", id);
            return await context.AppointmentServices
                .FromSqlRaw($"""SELECT * FROM "AppointmentServices" WHERE "ServiceId" = @ServiceId""", ServiceId)
                .ToListAsync();
        }

        public async Task AddAsync(AppointmentService appointmentService)
        {
            var parameters = RepositoryUtils.ParametersGenerator(appointmentService);
            await context.Database.ExecuteSqlRawAsync($"""
                INSERT INTO "AppointmentServices" ("Result", "AppointmentId", "ServiceId")
                VALUES (@Result, @AppointmentId, @ServiceId)
                """, parameters);
        }

        public async Task UpdateAsync(AppointmentService appointmentService)
        {
            var AppointmentServiceId = new NpgsqlParameter("AppointmentServiceId", appointmentService.AppointmentServiceId);
            var _appointmentService = await context.AppointmentServices.FromSqlRaw($"""SELECT * FROM "AppointmentServices" WHERE "AppointmentServiceId" = @AppointmentServiceId""", AppointmentServiceId)
                                                  .FirstOrDefaultAsync();
            if (_appointmentService != null)
            {
                var parameters = RepositoryUtils.ParametersGenerator(appointmentService);
                await context.Database.ExecuteSqlRawAsync($"""
                    UPDATE "AppointmentServices"
                    SET "Result" = @Result, "AppointmentId" = @AppointmentId, "ServiceId" = @ServiceId
                    WHERE "AppointmentServiceId" = @AppointmentServiceId
                    """, parameters);
            }
        }

        public async Task DeleteAsync(int id)
        {
            var AppointmentServiceId = new NpgsqlParameter("AppointmentServiceId", id);
            var appointmentService = await context.AppointmentServices.FromSqlRaw($"""SELECT * FROM "AppointmentServices" WHERE "AppointmentServiceId" = @AppointmentServiceId""", AppointmentServiceId).FirstOrDefaultAsync();

            if (appointmentService != null)
            {
                await context.Database.ExecuteSqlRawAsync($"""DELETE FROM "AppointmentServices" WHERE "AppointmentServiceId" = @AppointmentServiceId""", AppointmentServiceId);
            }
        }
    }
}
