using System.ComponentModel.DataAnnotations;

namespace Task10.Models;

public class Patient
{
    [Key]
    public int IdPatient { get; set; }

    [Required]
    [StringLength(100)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(100)]
    public string LastName { get; set; }

    [Required]
    public DateTime BirthDate { get; set; }

    public ICollection<Prescription> Prescriptions { get; set; }
}