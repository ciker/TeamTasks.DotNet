using CoreLibrary;
using System.Threading.Tasks;

namespace TeamTasks
{
    public interface ITeamTaskStore<TTeamTask> : IAsyncStore<TTeamTask, int>
        where TTeamTask : class, ITeamTask
    {
        Task<IProject> FindProjectByIdAsync(int projectId);
        Task<IAssignment> FindAssignmentAsync(int teamTaskId, int assigneeId);
        Task CreateAssignmentAsync(int teamTaskId, int assignorId, int? assigneeId, string description);
    }
}
