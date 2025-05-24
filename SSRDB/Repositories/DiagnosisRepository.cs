using SSRDB.Entities;
using SSRDB.Utils;
using SSRDB.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using SSRDB.Repositories.Interfaces;

namespace SSRDB.Repositories
{
    public class DiagnosisRepository(ApplicationDbContext context) : IDiagnosisRepository
    {

        public async Task<IEnumerable<Diagnosis>> GetAllAsync()
        {
            return await context.Diagnoses
                .FromSqlRaw($"""SELECT * FROM "Diagnoses" """)
                .ToListAsync();
        }

        public async Task<Diagnosis> GetByIdAsync(int id)
        {
            var DiagnosisId = new NpgsqlParameter("DiagnosisId", id);
            return await context.Diagnoses
                .FromSqlRaw($"""SELECT * FROM "Diagnoses" WHERE "DiagnosisId" = @DiagnosisId""", DiagnosisId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Diagnosis>> GetByAppointmentIdAsync(int id)
        {
            var AppointmentId = new NpgsqlParameter("AppointmentId", id);
            return await context.Diagnoses
                .FromSqlRaw($"""SELECT * FROM "Diagnoses" WHERE "AppointmentId" = @AppointmentId""", AppointmentId)
                .ToListAsync();
        }

        public async Task AddAsync(Diagnosis diagnosis)
        {
            var parameters = RepositoryUtils.ParametersGenerator(diagnosis);
            await context.Database.ExecuteSqlRawAsync($"""
            INSERT INTO "Diagnoses" ("DiagnosisCode", "Description", "Recommendations", "AppointmentId")
            VALUES (@DiagnosisCode, @Description, @Recommendations, @AppointmentId)
            """, parameters);
        }

        public async Task UpdateAsync(Diagnosis diagnosis)
        {
            var DiagnosisId = new NpgsqlParameter("DiagnosisId", diagnosis.DiagnosisId);
            var _diagnosis = await context.Diagnoses.FromSqlRaw($"""SELECT * FROM "Diagnosis" WHERE "DiagnosisId" = @DiagnosisId""", DiagnosisId)
                                                    .FirstOrDefaultAsync();
            if (_diagnosis != null)
            {
                var parameters = RepositoryUtils.ParametersGenerator(diagnosis);
                await context.Database.ExecuteSqlRawAsync($"""
                UPDATE "Diagnoses"
                SET "DiagnosisCode" = @DiagnosisCode, "Description" = @Description, "Recommendations" = @Recommendations, "AppointmentId" = @AppointmentId
                WHERE "DiagnosisId" = @DiagnosisId
                """, parameters);
            }
        }

        public async Task DeleteAsync(int id)
        {
            var DiagnosisId = new NpgsqlParameter("DiagnosisId", id);
            var diagnosis = await context.Diagnoses.FromSqlRaw($"""SELECT * FROM "Diagnoses" WHERE "DiagnosisId" = @DiagnosisId""", DiagnosisId).FirstOrDefaultAsync();

            if (diagnosis != null)
            {
                await context.Database.ExecuteSqlRawAsync($"""DELETE FROM "Diagnoses" WHERE "DiagnosisId" = @DiagnosisId""", DiagnosisId);
            }
        }
    }
}
