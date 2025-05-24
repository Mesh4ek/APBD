namespace Test.Exceptions;

public class TaskTypeNotFoundException(int taskTypeId)
    : Exception($"Task type with id: {taskTypeId} does not exist")
{
}
