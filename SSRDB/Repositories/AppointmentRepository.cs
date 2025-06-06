﻿using SSRDB.Entities;
using SSRDB.Utils;
using SSRDB.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using SSRDB.Repositories.Interfaces;

namespace SSRDB.Repositories
{
    public class AppointmentRepository(ApplicationDbContext context) : IAppointmentRepository
    {
        private readonly Dictionary<string, string> columnTranslate = new()
        {
            {"ID", "AppointmentId"},
            {"Пациент", "PatientId"},
            {"Врач", "EmployeeId"},
            {"Дата", "AppointmentDate"},
            {"Статус", "Status"},
        };
        public async Task<IEnumerable<Appointment>> GetAllAsync()
        {
            return await context.Appointments
                .FromSqlRaw($"""SELECT * FROM "Appointments" """)
                .ToListAsync();
        }
            
        public async Task<Appointment> GetByIdAsync(int id)
        {   
            var AppointmentId = new NpgsqlParameter("AppointmentId", id);
            return await context.Appointments
                .FromSqlRaw($"""SELECT * FROM "Appointments" WHERE "AppointmentId" = @AppointmentId""", AppointmentId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Appointment>> GetByPatientIdAsync(int id)
        {
            var PatientId = new NpgsqlParameter("PatientId", id);
            return await context.Appointments
                .FromSqlRaw($"""SELECT * FROM "Appointments" WHERE "PatientId" = @PatientId""", PatientId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetByPatientNameAsync(string name)
        {
            return await context.Appointments
                .FromSqlRaw($"""
                    SELECT a.*
                    FROM "Appointments" a
                    JOIN "Patients" p ON a."PatientId" = p."PatientId"
                    WHERE p."FullName" LIKE '%{name}%'
                """)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetByEmployeeNameAsync(string name)
        {
            return await context.Appointments
                .FromSqlRaw($"""
                    SELECT a.*
                    FROM "Appointments" a
                    JOIN "Employees" e ON a."EmployeeId" = e."EmployeeId"
                    WHERE e."FullName" LIKE '%{name}%'
                """)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetByEmployeeIdAsync(int id)
        {
            var EmployeeId = new NpgsqlParameter("EmployeeId", id);
            return await context.Appointments
                .FromSqlRaw($"""SELECT * FROM "Appointments" WHERE "EmployeeId" = @EmployeeId""", EmployeeId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>?> SortByColumn(string column, string method)
        {
            if (!columnTranslate.TryGetValue(column, out string? columnName))
            {
                return null;
            }
            var safeColumnName = $"\"{columnName}\"";

            return await context.Appointments
                .FromSqlRaw($"""SELECT * FROM "Appointments" ORDER BY {safeColumnName} {method}""")
                .ToListAsync();
        }
        public async Task AddAsync(Appointment appointment)
        {
            var parameters = RepositoryUtils.ParametersGenerator(appointment);
            await context.Database.ExecuteSqlRawAsync($"""
                INSERT INTO "Appointments" ("AppointmentDate", "Status", "Notes", "PatientId", "EmployeeId")
                VALUES (@AppointmentDate, @Status, @Notes, @PatientId, @EmployeeId)
                """, parameters);
        }

        public async Task UpdateAsync(Appointment appointment)
        {
            var AppointmentId = new NpgsqlParameter("AppointmentId", appointment.AppointmentId);
            var _appointment = await context.Appointments.FromSqlRaw($"""SELECT * FROM "Appointments" WHERE "AppointmentId" = @AppointmentId""", AppointmentId)
                                                  .FirstOrDefaultAsync();
            if (_appointment != null)
            {
                var parameters = RepositoryUtils.ParametersGenerator(appointment);
                await context.Database.ExecuteSqlRawAsync($"""
                    UPDATE "Appointments"
                    SET "Status" = @Status, "Notes" = @Notes, "PatientId" = @PatientId, "EmployeeId" = @EmployeeId
                    WHERE "AppointmentId" = @AppointmentId
                    """, parameters);
            }
        }

        public async Task DeleteAsync(int id)
        {
            var AppointmentId = new NpgsqlParameter("AppointmentId", id);
            var appointment = await context.Appointments.FromSqlRaw($"""SELECT * FROM "Appointments" WHERE "AppointmentId" = @AppointmentId""", AppointmentId).FirstOrDefaultAsync();

            if (appointment != null)
            {
                await context.Database.ExecuteSqlRawAsync($"""DELETE FROM "Appointments" WHERE "AppointmentId" = @AppointmentId""", AppointmentId);
            }
        }
    }
}
