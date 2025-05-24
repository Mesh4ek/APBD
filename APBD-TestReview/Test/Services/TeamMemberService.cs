using Test.Entities;
using Test.Exceptions;
using Test.Infrastructure;
using Test.Infrastructure.Repositories.Abstractions;
using Test.Services.Abstractions;

namespace Test.Services;

public class TeamMemberService : ITeamMemberService
{
    private readonly ITeamMemberRepository _teamMemberRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TeamMemberService(ITeamMemberRepository teamMemberRepository, IUnitOfWork unitOfWork)
    {
        _teamMemberRepository = teamMemberRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<TeamMember> GetTeamMemberWithTasksAsync(int teamMemberId, CancellationToken token = default)
    {
        var member = await _teamMemberRepository.GetTeamMemberWithTasksAsync(teamMemberId, token);
        if (member is null)
            throw new TeamMemberNotFoundException(teamMemberId);

        return member;
    }
}