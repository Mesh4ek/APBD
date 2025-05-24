using Test.Entities;

namespace Test.Infrastructure.Repositories.Abstractions;

public interface ITeamMemberRepository
{
    Task<TeamMember?> GetTeamMemberWithTasksAsync(int teamMemberId, CancellationToken token = default);
}