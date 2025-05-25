using SSRDB.Entities;
using Microsoft.EntityFrameworkCore;

namespace SSRDB.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Diagnosis> Diagnoses { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<AppointmentService> AppointmentServices { get; set; }
        public DbSet<Medication> Medications { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
    }
}
