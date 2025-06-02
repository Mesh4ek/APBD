using System.ComponentModel.DataAnnotations;

namespace Task10.DTOs;

public class PatientDTO
{
    public int? IdPatient { get; set; }

    [Required]
    [StringLength(100)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(100)]
    public string LastName { get; set; }

    [Required]
    public DateTime BirthDate { get; set; }
}