using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Test.Contracts.Requests;

public class CreateTaskRequest
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [StringLength(100)]
    public string Description { get; set; } = string.Empty;
    
    [Required]
    public DateTime Deadline { get; set; }
 
    [Required]
    [Range(1, int.MaxValue)]
    public int IdProject { get; set; }
    
    [Required]
    [Range(1, int.MaxValue)]
    public int IdTaskType { get; set; }
    
    [Required]
    [Range(1, int.MaxValue)]
    public int IdAssignedTo { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int IdCreator { get; set; }
}
