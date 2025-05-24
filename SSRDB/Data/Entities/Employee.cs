using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSRDB.Entities
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeId { get; set; }
        public string FullName { get; set; }
        public string Specialization { get; set; }
        public string LicenseNumber { get; set; }
        public string Phone { get; set; }

        virtual public ICollection<Appointment> Appointments { get; set; }
    }
}
