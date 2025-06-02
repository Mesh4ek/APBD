using System.ComponentModel.DataAnnotations;

namespace Task10.DTOs;

public class PrescriptionRequestDTO
{
    [Required]
    public DateTime Date { get; set; }

    [Required]
    public DateTime DueDate { get; set; }

    [Required]
    public PatientDTO Patient { get; set; }

    [Required]
    public int DoctorId { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(10)]
    public List<PrescriptionMedicamentDTO> Medicaments { get; set; }
}