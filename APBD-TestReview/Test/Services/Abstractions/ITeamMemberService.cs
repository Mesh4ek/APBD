using Test.Entities;

namespace Test.Services.Abstractions;

public interface ITeamMemberService
{
    Task<TeamMember> GetTeamMemberWithTasksAsync(int teamMemberId, CancellationToken token = default);
}