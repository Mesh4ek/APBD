using Task = Test.Entities.Task;

namespace Test.Infrastructure.Repositories.Abstractions;

public interface ITaskRepository
{
    Task<Task> CreateTaskAsync(Task task, CancellationToken token = default);

    Task<bool> ProjectExistsAsync(int requestIdProject, CancellationToken token = default);
    Task<bool> TaskTypeExistsAsync(int requestIdTaskType, CancellationToken token = default);
    Task<bool> TeamMemberExistsAsync(int requestIdAssignedTo, CancellationToken token = default);
}