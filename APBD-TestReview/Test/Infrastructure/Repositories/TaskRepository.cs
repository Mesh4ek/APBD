using Test.Infrastructure.Repositories.Abstractions;
using TaskEntity = Test.Entities.Task;

namespace Test.Infrastructure;

public class TaskRepository : ITaskRepository
{
    
    private readonly IUnitOfWork _uow;

    public TaskRepository(IUnitOfWork uow)
    {
        _uow = uow;
    }
    
    public async Task<TaskEntity> CreateTaskAsync(TaskEntity task, CancellationToken token = default)
    {
        const string query = """
                             INSERT INTO Task(Name, Description, Deadline, IdProject, IdTaskType, IdAssignedTo, IdCreator)
                             VALUES (@name, @description, @deadline, @idProject, @idTaskType, @idAssignedTo, @idCreator)
                             SELECT SCOPE_IDENTITY();
                             """;

        var con = await _uow.GetConnectionAsync();
        await using var cmd = con.CreateCommand();
        cmd.CommandText = query;
        cmd.Transaction = _uow.Transaction;
        cmd.Parameters.AddWithValue("@name", task.Name);
        cmd.Parameters.AddWithValue("@description", task.Description);
        cmd.Parameters.AddWithValue("@deadline", task.Deadline);
        cmd.Parameters.AddWithValue("@idProject", task.IdProject);
        cmd.Parameters.AddWithValue("@idTaskType", task.IdTaskType);
        cmd.Parameters.AddWithValue("@idAssignedTo", task.IdAssignedTo);
        cmd.Parameters.AddWithValue("@idCreator", task.IdCreator);
        
        
        var result = await cmd.ExecuteScalarAsync(token);
        task.Id = Convert.ToInt32(result);
        return task;
    }

    public async Task<bool> ProjectExistsAsync(int requestIdProject, CancellationToken token = default)
    {
        const string query = """
                             SELECT 
                                 IIF(EXISTS (SELECT 1 FROM Project 
                                         WHERE Project.IdProject = @requestIdProject), 1, 0);   
                             """;

        var con = await _uow.GetConnectionAsync();
        await using var cmd = con.CreateCommand();
        cmd.CommandText = query;
        cmd.Transaction = _uow.Transaction;
        cmd.Parameters.AddWithValue("@requestIdProject", requestIdProject);
        
        var result = Convert.ToInt32(await cmd.ExecuteScalarAsync(token));
        return result == 1;
    }

    public async Task<bool> TaskTypeExistsAsync(int requestIdTaskType, CancellationToken token = default)
    {
        const string query = """
                             SELECT 
                                 IIF(EXISTS (SELECT 1 FROM TaskType 
                                         WHERE TaskType.IdTaskType = @requestIdTaskType), 1, 0);   
                             """;

        var con = await _uow.GetConnectionAsync();
        await using var cmd = con.CreateCommand();
        cmd.CommandText = query;
        cmd.Transaction = _uow.Transaction;
        cmd.Parameters.AddWithValue("@requestIdTaskType", requestIdTaskType);
        
        var result = Convert.ToInt32(await cmd.ExecuteScalarAsync(token));
        return result == 1;
    }

    public async Task<bool> TeamMemberExistsAsync(int requestId, CancellationToken token = default)
    {
        const string query = """
                             SELECT 
                                 IIF(EXISTS (SELECT 1 FROM TeamMember 
                                         WHERE TeamMember.IdTeamMember = @requestId), 1, 0);   
                             """;

        var con = await _uow.GetConnectionAsync();
        await using var cmd = con.CreateCommand();
        cmd.CommandText = query;
        cmd.Transaction = _uow.Transaction;
        cmd.Parameters.AddWithValue("@requestId", requestId);
        
        var result = Convert.ToInt32(await cmd.ExecuteScalarAsync(token));
        return result == 1;
    }
}