using System.ComponentModel.DataAnnotations;

namespace Task10.Models;

public class Prescription
{
    [Key]
    public int IdPrescription { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [Required]
    public DateTime DueDate { get; set; }

    [Required]
    public int IdDoctor { get; set; }
    public Doctor Doctor { get; set; }

    [Required]
    public int IdPatient { get; set; }
    public Patient Patient { get; set; }

    public ICollection<PrescriptionMedicament> PrescriptionMedicaments { get; set; }
}