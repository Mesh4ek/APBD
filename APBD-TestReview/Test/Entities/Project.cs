namespace Test.Entities;

public class Project : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public DateTime Deadline { get; set; }
    public List<Task> Tasks { get; set; } = [];
}