using System.ComponentModel.DataAnnotations;

namespace Task10.DTOs;

public class PrescriptionMedicamentDTO
{
    [Required]
    public int IdMedicament { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int Dose { get; set; }

    [Required]
    [StringLength(100)]
    public string Details { get; set; }
}
