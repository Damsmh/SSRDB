using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSRDB.Entities
{
    public class Prescription
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PrescriptionId { get; set; }
        public string Dosage { get; set; }
        public int DurationDays { get; set; }

        public int DiagnosisId { get; set; }
        public int MedicationId { get; set; }

        virtual public Diagnosis Diagnosis { get; set; }
        virtual public Medication Medication { get; set; }
    }
}
