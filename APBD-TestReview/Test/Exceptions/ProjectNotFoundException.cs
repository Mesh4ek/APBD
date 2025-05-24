namespace Test.Exceptions;

public class ProjectNotFoundException(int projectId) 
    : Exception($"Project with id: {projectId} does not exist")
{
}