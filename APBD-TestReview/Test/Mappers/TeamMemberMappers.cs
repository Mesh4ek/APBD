using Test.Contracts.Responses;
using Test.Entities;

namespace Test.Mappers;


public static class TeamMemberMappers
{
    public static GetTeamMemberResponse MapToGetTeamMemberResponse(this TeamMember member)
    {
        return new GetTeamMemberResponse
        {
            Id = member.Id,
            FirstName = member.FirstName,
            LastName = member.LastName,
            Email = member.Email,
            AssignedTasks = member.AssignedTasks.Select(t => new TaskInfoResponse
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                Deadline = t.Deadline,
                ProjectName = t.Project?.Name ?? string.Empty,
                TaskTypeName = t.TaskType?.Name ?? string.Empty
            }).ToList(),
            CreatedTasks = member.CreatedTasks.Select(t => new TaskInfoResponse
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                Deadline = t.Deadline,
                ProjectName = t.Project?.Name ?? string.Empty,
                TaskTypeName = t.TaskType?.Name ?? string.Empty
            }).ToList()
        };
    }
}