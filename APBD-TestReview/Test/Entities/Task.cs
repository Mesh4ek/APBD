namespace Test.Entities;

public class Task : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime Deadline { get; set; }
    public int IdProject { get; set; }
    public Project Project { get; set; } = null!;

    public int IdTaskType { get; set; }
    public TaskType TaskType { get; set; } = null!;

    public int IdAssignedTo { get; set; }
    public TeamMember AssignedTo { get; set; } = null!;

    public int IdCreator { get; set; }
    public TeamMember Creator { get; set; } = null!;
}