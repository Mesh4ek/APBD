namespace Test.Exceptions;

public class TeamMemberNotFoundException(int teamMemberId)
    : Exception($"Team member with id: {teamMemberId} does not exist")
{
}