using Test.Entities;
using Test.Infrastructure.Repositories.Abstractions;
using Task = Test.Entities.Task;

namespace Test.Infrastructure.Repositories;

public class TeamMemberRepository : ITeamMemberRepository
{
    private readonly IUnitOfWork _uow;

    public TeamMemberRepository(IUnitOfWork uow)
    {
        _uow = uow;
    }
    public async Task<TeamMember?> GetTeamMemberWithTasksAsync(int teamMemberId, CancellationToken token = default)
    {
        const string query = """
                             SELECT
                                 TM.IdTeamMember,
                                 TM.FirstName,
                                 TM.LastName,
                                 TM.Email,
                             
                                 T.IdTask AS TaskId,
                                 T.Name,
                                 T.Description,
                                 T.Deadline,
                                 T.IdProject,
                                 P.Name AS ProjectName,
                                 T.IdTaskType,
                                 TT.Name AS TaskTypeName,
                                 T.IdAssignedTo,
                                 T.IdCreator
                             FROM TeamMember TM
                                      LEFT JOIN Task T ON T.IdAssignedTo = TM.IdTeamMember OR T.IdCreator = TM.IdTeamMember
                                      LEFT JOIN Project P ON P.IdProject = T.IdProject
                                      LEFT JOIN TaskType TT ON TT.IdTaskType = T.IdTaskType
                             WHERE TM.IdTeamMember = @teamMemberId;
                             """;
        var con = await _uow.GetConnectionAsync();
        await using var cmd = con.CreateCommand();
        cmd.CommandText = query;
        cmd.Transaction = _uow.Transaction;
        cmd.Parameters.AddWithValue("@teamMemberId", teamMemberId);
        
        var reader = await cmd.ExecuteReaderAsync(token);
         if (!reader.HasRows)
             return null;

         TeamMember? teamMember = null;
         var assignedTasks = new List<Task>();
         var createdTasks = new List<Task>();

         while (await reader.ReadAsync(token))
         {
             if (teamMember == null)
             {
                 teamMember = new TeamMember
                 {
                     Id = reader.GetInt32(0),
                     FirstName = reader.GetString(1),
                     LastName = reader.GetString(2),
                     Email = reader.GetString(3),
                     AssignedTasks = [],
                     CreatedTasks = []
                 };
             }

             if (!reader.IsDBNull(4)) // TaskId is not null
             {
                 var task = new Task
                 {
                     Id = reader.GetInt32(4),
                     Name = reader.GetString(5),
                     Description = reader.GetString(6),
                     Deadline = reader.GetDateTime(7),
                     Project = new Project
                     {
                         Id = reader.GetInt32(8),
                         Name = reader.GetString(9)
                     },
                     TaskType = new TaskType
                     {
                         Id = reader.GetInt32(10),
                         Name = reader.GetString(11)
                     },
                     IdAssignedTo = reader.GetInt32(12),
                     IdCreator = reader.GetInt32(13)
                 };

                 var currentId = teamMember.Id;

                 if (task.IdAssignedTo == currentId)
                     assignedTasks.Add(task);
                 if (task.IdCreator == currentId)
                     createdTasks.Add(task);
             }
         }
         
         teamMember.AssignedTasks = assignedTasks.OrderByDescending(t => t.Deadline).ToList();
         teamMember.CreatedTasks = createdTasks.OrderByDescending(t => t.Deadline).ToList();

         return teamMember;
        
    }
}