using Test.Contracts.Requests;

namespace Test.Services.Abstractions;

public interface ITaskService
{
    public Task<int> CreateTaskAsync(CreateTaskRequest task, CancellationToken token = default);
}