using SSRDB.Entities;
using SSRDB.Utils;
using SSRDB.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using SSRDB.Repositories.Interfaces;

namespace SSRDB.Repositories
{
    public class PrescriptionRepository(ApplicationDbContext context) : IPrescriptionRepository
    {
        private readonly Dictionary<string, string> columnTranslate = new()
        {
            {"ID", "PrescriprionId"},
            {"Код диагноза(МКБ-10)", "DiagnosisId"},
            {"Медикамент", "MedicationId"},
            {"Доза", "Dosage"},
        };
        public async Task<IEnumerable<Prescription>> GetAllAsync()
        {
            return await context.Prescriptions
                .FromSqlRaw($"""SELECT * FROM "Prescriptions" """)
                .ToListAsync();
        }

        public async Task<Prescription> GetByIdAsync(int id)
        {
            var PrescriptionId = new NpgsqlParameter("PrescriptionId", id);
            return await context.Prescriptions
                .FromSqlRaw($"""SELECT * FROM "Prescriptions" WHERE "PrescriptionId" = @PrescriptionId""", PrescriptionId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Prescription>> GetByDiagnosisIdAsync(int id)
        {
            var DiagnosisId = new NpgsqlParameter("DiagnosisId", id);
            return await context.Prescriptions
                .FromSqlRaw($"""SELECT * FROM "Prescriptions" WHERE "DiagnosisId" = @DiagnosisId""", DiagnosisId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Prescription>> GetByMedicationIdAsync(int id)
        {
            var MedicationId = new NpgsqlParameter("MedicationId", id);
            return await context.Prescriptions
                .FromSqlRaw($"""SELECT * FROM "Prescriptions" WHERE "MedicationId" = @MedicationId""", MedicationId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Prescription>?> SortByColumn(string column, string method)
        {
            if (!columnTranslate.TryGetValue(column, out string? columnName))
            {
                return null;
            }
            var safeColumnName = $"\"{columnName}\"";

            return await context.Prescriptions
                .FromSqlRaw($"""SELECT * FROM "Prescriptions" ORDER BY {safeColumnName} {method}""")
                .ToListAsync();
        }

        public async Task AddAsync(Prescription prescription)
        {
            var parameters = RepositoryUtils.ParametersGenerator(prescription);
            await context.Database.ExecuteSqlRawAsync($"""
                INSERT INTO "Prescriptions" ("Dosage", "DurationDays", "DiagnosisId", "MedicationId")
                VALUES (@Dosage, @DurationDays, @DiagnosisId, @MedicationId)
                """, parameters);
        }

        public async Task UpdateAsync(Prescription prescription)
        {
            var PrescriptionId = new NpgsqlParameter("PrescriptionId", prescription.PrescriptionId);
            var _prescription = await context.Prescriptions.FromSqlRaw($"""SELECT * FROM "Prescriptions" WHERE "PrescriptionId" = @PrescriptionId""", PrescriptionId)
                                                  .FirstOrDefaultAsync();
            if (_prescription != null)
            {
                var parameters = RepositoryUtils.ParametersGenerator(prescription);
                await context.Database.ExecuteSqlRawAsync($"""
                    UPDATE "Prescriptions"
                    SET "Dosage" = @Dosage, "DurationDays" = @DurationDays, "DiagnosisId" = @DiagnosisId, "MedicationId" = @MedicationId
                    WHERE "PrescriptionId" = @PrescriptionId
                    """, parameters);
            }
        }

        public async Task DeleteAsync(int id)
        {
            var PrescriptionId = new NpgsqlParameter("PrescriptionId", id);
            var prescription = await context.Prescriptions.FromSqlRaw($"""SELECT * FROM "Prescriptions" WHERE "PrescriptionId" = @PrescriptionId""", PrescriptionId).FirstOrDefaultAsync();

            if (prescription != null)
            {
                await context.Database.ExecuteSqlRawAsync($"""DELETE FROM "Prescriptions" WHERE "PrescriptionId" = @PrescriptionId""", PrescriptionId);
            }
        }
    }
}
