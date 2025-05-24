using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSRDB.Entities
{
    public class Diagnosis
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DiagnosisId { get; set; }
        [Required] public string DiagnosisCode { get; set; }
        [Required] public string Description { get; set; }
        [Required] public string Recommendations { get; set; }
        [Required] public int AppointmentId { get; set; }

        virtual public Appointment Appointment { get; set; }
        virtual public ICollection<Prescription> Prescriptions { get; set; }
    }
}
