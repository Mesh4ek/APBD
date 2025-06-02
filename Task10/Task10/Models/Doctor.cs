using System.ComponentModel.DataAnnotations;

namespace Task10.Models;

public class Doctor
{
    [Key]
    public int IdDoctor { get; set; }

    [Required]
    [StringLength(100)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(100)]
    public string LastName { get; set; }

    [Required]
    [StringLength(100)]
    public string Email { get; set; }

    public ICollection<Prescription> Prescriptions { get; set; }
}