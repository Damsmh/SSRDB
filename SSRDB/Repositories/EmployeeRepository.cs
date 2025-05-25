using SSRDB.Entities;
using SSRDB.Utils;
using SSRDB.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using SSRDB.Repositories.Interfaces;

namespace SSRDB.Repositories
{
    public class EmployeeRepository(ApplicationDbContext context) : IEmployeeRepository
    {
        private readonly Dictionary<string, string> columnTranslate = new()
        {
            {"ID", "EmployeeId"},
            {"ФИО", "FullName"},
            {"Специализация", "Specialization"},
        };
        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await context.Employees
                .FromSqlRaw($"""SELECT * FROM "Employees" """)
                .ToListAsync();
        }

        public async Task<Employee> GetByIdAsync(int id)
        {
            var EmployeeId = new NpgsqlParameter("EmployeeId", id);
            return await context.Employees
                .FromSqlRaw($"""SELECT * FROM "Employees" WHERE "EmployeeId" = @EmployeeId""", EmployeeId)
                .FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<Employee>> GetByNameAsync(string name)
        {
            var EmployeeName = new NpgsqlParameter("FullName", name);
            return await context.Employees
                .FromSqlRaw($"""SELECT * FROM "Employees" WHERE "FullName" LIKE @EmployeeId""", EmployeeName)
                .ToListAsync();
        }

        public async Task AddAsync(Employee employee)
        {
            var parameters = RepositoryUtils.ParametersGenerator(employee);
            await context.Database.ExecuteSqlRawAsync($"""
                INSERT INTO "Employees" ("FullName", "Specialization", "LicenseNumber", "Phone")
                VALUES (@FullName, @Specialization, @LicenseNumber, @Phone)
                """, parameters);
        }

        public async Task UpdateAsync(Employee employee)
        {
            var EmployeeId = new NpgsqlParameter("EmployeeId", employee.EmployeeId);
            var _employee = await context.Employees.FromSqlRaw($"""SELECT * FROM "Employees" WHERE "EmployeeId" = @EmployeeId""", EmployeeId)
                                                  .FirstOrDefaultAsync();
            if (_employee != null)
            {
                var parameters = RepositoryUtils.ParametersGenerator(employee);
                await context.Database.ExecuteSqlRawAsync($"""
                    UPDATE "Employees"
                    SET "FullName" = @FullName, "Specialization" = @Specialization, "LicenseNumber" = @LicenseNumber, "Phone" = @Phone
                    WHERE "EmployeeId" = @EmployeeId
                    """, parameters);
            }
        }

        public async Task<IEnumerable<Employee>?> SortByColumn(string column, string method)
        {
            if (!columnTranslate.TryGetValue(column, out string? columnName))
            {
                return null;
            }
            var safeColumnName = $"\"{columnName}\"";

            return await context.Employees
                .FromSqlRaw($"""SELECT * FROM "Employees" ORDER BY {safeColumnName} {method}""")
                .ToListAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var EmployeeId = new NpgsqlParameter("EmployeeId", id);
            var employee = await context.Employees.FromSqlRaw($"""SELECT * FROM "Employees" WHERE "EmployeeId" = @EmployeeId""", EmployeeId).FirstOrDefaultAsync();

            if (employee != null)
            {
                await context.Database.ExecuteSqlRawAsync($"""DELETE FROM "Employees" WHERE "EmployeeId" = @EmployeeId""", EmployeeId);
            }
        }
    }
}
