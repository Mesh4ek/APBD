using System.ComponentModel.DataAnnotations;

namespace Task10.Models;

public class Medicament
{
    [Key]
    public int IdMedicament { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [StringLength(1000)]
    public string Description { get; set; }

    [StringLength(100)]
    public string Type { get; set; }
    
    public ICollection<PrescriptionMedicament> PrescriptionMedicaments { get; set; }
}