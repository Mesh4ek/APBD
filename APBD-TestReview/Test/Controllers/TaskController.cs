using Microsoft.AspNetCore.Mvc;
using Test.Contracts.Requests;
using Test.Exceptions;
using Test.Services.Abstractions;

namespace Test.Controllers;

[ApiController]
[Route("api/tasks")]
public class TaskController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TaskController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateTaskAsync([FromBody] CreateTaskRequest request, CancellationToken token = default)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var taskId = await _taskService.CreateTaskAsync(request, token);
            return StatusCode(StatusCodes.Status201Created, new { taskId });
        }
        catch (ProjectNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (TaskTypeNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (TeamMemberNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return CreateProblemResult(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    private ObjectResult CreateProblemResult(int statusCode, string detail)
    {
        return new ObjectResult(new ProblemDetails
        {
            Status = statusCode,
            Detail = detail
        })
        {
            StatusCode = statusCode
        };
    }
}