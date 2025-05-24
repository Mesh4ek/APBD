namespace Test.Contracts.Responses;

public record struct TaskInfoResponse(
    int Id,
    string Name,
    string Description,
    DateTime Deadline,
    string ProjectName,
    string TaskTypeName
);
