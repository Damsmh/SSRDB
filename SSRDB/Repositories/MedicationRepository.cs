using SSRDB.Entities;
using SSRDB.Utils;
using SSRDB.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using SSRDB.Repositories.Interfaces;

namespace SSRDB.Repositories
{
    public class MedicationRepository(ApplicationDbContext context) : IMedicationRepository
    {
        private readonly Dictionary<string, string> columnTranslate = new()
        {
            {"ID", "MMedicationid"},
            {"Название", "Name"},
            {"Производитель", "Manufacturer"},
            {"Цена", "Price"},
        };
        public async Task<IEnumerable<Medication>> GetAllAsync()
        {
            return await context.Medications
                .FromSqlRaw($"""SELECT * FROM "Medications" """)
                .ToListAsync();
        }

        public async Task<Medication> GetByIdAsync(int id)
        {
            var MedicationId = new NpgsqlParameter("MedicationId", id);
            return await context.Medications
                .FromSqlRaw($"""SELECT * FROM "Medications" WHERE "MedicationId" = @MedicationId""", MedicationId)
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync(Medication Medication)
        {
            var parameters = RepositoryUtils.ParametersGenerator(Medication);
            await context.Database.ExecuteSqlRawAsync($"""
                INSERT INTO "Medications" ("Name", "Manufacturer", "Price")
                VALUES (@Name, @Manufacturer, @Price)
                """, parameters);
        }

        public async Task UpdateAsync(Medication Medication)
        {
            var MedicationId = new NpgsqlParameter("MedicationId", Medication.MedicationId);
            var _Medication = await context.Medications.FromSqlRaw($"""SELECT * FROM "Medications" WHERE "MedicationId" = @MedicationId""", MedicationId)
                                                  .FirstOrDefaultAsync();

            if (_Medication != null)
            {
                var parameters = RepositoryUtils.ParametersGenerator(Medication);
                await context.Database.ExecuteSqlRawAsync($"""
                    UPDATE "Medications"
                    SET "Name" = @Name, "Manufacturer" = @Manufacturer, "Price" = @Price
                    WHERE "MedicationId" = @MedicationId
                    """, parameters);
            }
        }

        public async Task<IEnumerable<Medication>?> SortByColumn(string column, string method)
        {
            if (!columnTranslate.TryGetValue(column, out string? columnName))
            {
                return null;
            }
            var safeColumnName = $"\"{columnName}\"";

            return await context.Medications
                .FromSqlRaw($"""SELECT * FROM "Medications" ORDER BY {safeColumnName} {method}""")
                .ToListAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var MedicationId = new NpgsqlParameter("MedicationId", id);
            var Medication = await context.Medications.FromSqlRaw($"""SELECT * FROM "Medications" WHERE "MedicationId" = @MedicationId""", MedicationId).FirstOrDefaultAsync();

            if (Medication != null)
            {
                await context.Database.ExecuteSqlRawAsync($"""DELETE FROM "Medications" WHERE "MedicationId" = @MedicationId""", MedicationId);
            }
        }
    }
}
