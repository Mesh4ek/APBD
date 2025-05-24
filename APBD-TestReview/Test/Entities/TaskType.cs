namespace Test.Entities;

public class TaskType : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public List<Task> Tasks { get; set; } = [];
}