using Microsoft.EntityFrameworkCore;
using SSRDB.Components;
using SSRDB.Data;
using SSRDB.Repositories.Interfaces;
using SSRDB.Repositories;

namespace SSRDB
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            var initializer = new DatabaseInitializer(builder.Configuration.GetConnectionString("DefaultConnection"));
            await initializer.InitializeDatabaseAsync();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options
                .UseLazyLoadingProxies()
                .UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            builder.Services.AddScoped<IPatientRepository, PatientRepository>();
            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
            builder.Services.AddScoped<IDiagnosisRepository, DiagnosisRepository>();
            builder.Services.AddScoped<IMedicationRepository, MedicationRepository>();
            builder.Services.AddScoped<IPrescriptionRepository, PrescriptionRepository>();
            builder.Services.AddScoped<IAppointmentServiceRepository, AppointmentServiceRepository>();

            var app = builder.Build();



            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseAntiforgery();

            app.MapStaticAssets();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
