using Microsoft.AspNetCore.Mvc;
using Test.Contracts.Responses;
using Test.Exceptions;
using Test.Mappers;
using Test.Services.Abstractions;

namespace Test.Controllers;

[ApiController]
[Route("api/team-members")]
public class TeamMemberController : ControllerBase
{
    private readonly ITeamMemberService _teamMemberService;

    public TeamMemberController(ITeamMemberService teamMemberService)
    {
        _teamMemberService = teamMemberService;
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(GetTeamMemberResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetTeamMemberById([FromRoute] int id, CancellationToken token = default)
    {
        if (id <= 0)
            return BadRequest("Team member ID must be greater than 0.");
        
        try
        {
            var member = await _teamMemberService.GetTeamMemberWithTasksAsync(id, token);
            var response = member.MapToGetTeamMemberResponse();
            return Ok(response);
        }
        catch (TeamMemberNotFoundException)
        {
            return NotFound($"Team member with id {id} was not found.");
        }
    }
}
