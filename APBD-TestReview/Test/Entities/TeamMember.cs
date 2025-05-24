namespace Test.Entities;

public class TeamMember : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<Task> AssignedTasks { get; set; } = [];
    public List<Task> CreatedTasks { get; set; } = [];
}