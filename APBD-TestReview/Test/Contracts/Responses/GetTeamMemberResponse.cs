namespace Test.Contracts.Responses;

public record struct GetTeamMemberResponse(
    int Id,
    string FirstName,
    string LastName,
    string Email,
    List<TaskInfoResponse> AssignedTasks,
    List<TaskInfoResponse> CreatedTasks
);