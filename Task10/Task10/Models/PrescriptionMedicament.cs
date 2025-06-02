using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task10.Models;

public class PrescriptionMedicament
{
    public int IdPrescription { get; set; }
    public Prescription Prescription { get; set; }
    
    public int IdMedicament { get; set; }
    public Medicament Medicament { get; set; }
    
    [Required]
    [Range(1, int.MaxValue)]
    public int Dose { get; set; }

    [StringLength(100)]
    public string Details { get; set; }
}