using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSRDB.Entities
{
    public class Patient
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PatientId { get; set; }
        public string FullName { get; set; }
        public DateOnly BirthDate { get; set; } = new DateOnly(2005, 6, 21);
        public char Gender { get; set; }
        public string PassportNumber { get; set; }
        public string Phone { get; set; }
        public string? Email { get; set; }
        public string Address { get; set; }

        virtual public ICollection<Appointment> Appointments { get; set; }
    }
}
