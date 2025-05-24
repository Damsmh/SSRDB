using SSRDB.Entities;
using SSRDB.Utils;
using SSRDB.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using SSRDB.Repositories.Interfaces;


namespace SSRDB.Repositories
{
    public class ServiceRepository(ApplicationDbContext context) : IServiceRepository
    {
        public async Task<IEnumerable<Service>> GetAllAsync()
        {
            return await context.Services
                .FromSqlRaw($"""SELECT * FROM "Services" """)
                .ToListAsync();
        }

        public async Task<Service> GetByIdAsync(int id)
        {
            var ServiceId = new NpgsqlParameter("ServiceId", id);
            return await context.Services
                .FromSqlRaw($"""SELECT * FROM "Services" WHERE "ServiceId" = @ServiceId""", ServiceId)
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync(Service service)
        {
            var parameters = RepositoryUtils.ParametersGenerator(service); 
            await context.Database.ExecuteSqlRawAsync($"""
                INSERT INTO "Services" ("Name", "Price", "DurationMinutes")
                VALUES (@Name, @Price, @DurationMinutes)
                """, parameters);
        }

        public async Task UpdateAsync(Service service)
        {
            var ServiceId = new NpgsqlParameter("ServiceId", service.ServiceId);
            var _service = await context.Services.FromSqlRaw($"""SELECT * FROM "Services" WHERE "ServiceId" = @ServiceId""", ServiceId).FirstOrDefaultAsync();
            if (_service != null)
            {
                var parameters = RepositoryUtils.ParametersGenerator(service);
                await context.Database.ExecuteSqlRawAsync($"""
                    UPDATE "Services"
                    SET "Name" = @Name, "Price" = @Price, "DurationMinutes" = @DurationMinutes
                    WHERE "ServiceId" = @ServiceId
                    """, parameters);
            }
        }

        public async Task DeleteAsync(int id)
        {
            var ServiceId = new NpgsqlParameter("ServiceId", id);
            var service = await context.Services.FromSqlRaw($"""SELECT * FROM "Services" WHERE "ServiceId" = @ServiceId""", ServiceId).FirstOrDefaultAsync();
            if (service != null)
                await context.Database.ExecuteSqlRawAsync($"""DELETE FROM "Services" WHERE "ServiceId" = @ServiceId""", ServiceId);
        }
    }
}
