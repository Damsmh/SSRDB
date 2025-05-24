using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSRDB.Entities
{
    public class Appointment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AppointmentId { get; set; }
        public DateTime AppointmentDate { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Запланирован";
        public string? Notes { get; set; }

        public int PatientId { get; set; }
        public int EmployeeId { get; set; }

        virtual public Patient Patient { get; set; }
        virtual public Employee Employee { get; set; }
        virtual public Diagnosis Diagnosis { get; set; }
        virtual public ICollection<AppointmentService> AppointmentServices { get; set; }
    }
}
