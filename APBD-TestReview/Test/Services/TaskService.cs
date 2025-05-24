using Test.Contracts.Requests;
using Test.Exceptions;
using Test.Infrastructure;
using Test.Infrastructure.Repositories.Abstractions;
using Test.Services.Abstractions;
using Task = Test.Entities.Task;

namespace Test.Services;

public class TaskService : ITaskService
{
    
    private readonly ITaskRepository _taskRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TaskService(ITaskRepository taskRepository, IUnitOfWork unitOfWork)
    {
        _taskRepository = taskRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<int> CreateTaskAsync(CreateTaskRequest request, CancellationToken token = default)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            if (!await _taskRepository.ProjectExistsAsync(request.IdProject, token))
                throw new ProjectNotFoundException(request.IdProject);

            if (!await _taskRepository.TaskTypeExistsAsync(request.IdTaskType, token))
                throw new TaskTypeNotFoundException(request.IdTaskType);

            if (!await _taskRepository.TeamMemberExistsAsync(request.IdAssignedTo, token))
                throw new TeamMemberNotFoundException(request.IdAssignedTo);

            if (!await _taskRepository.TeamMemberExistsAsync(request.IdCreator, token))
                throw new TeamMemberNotFoundException(request.IdCreator);

            var newTask = new Task
            {
                Name = request.Name,
                Description = request.Description,
                Deadline = request.Deadline,
                IdProject = request.IdProject,
                IdTaskType = request.IdTaskType,
                IdAssignedTo = request.IdAssignedTo,
                IdCreator = request.IdCreator
            };

            var task = await _taskRepository.CreateTaskAsync(newTask, token);
            await _unitOfWork.CommitTransactionAsync();
            return task.Id;
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}