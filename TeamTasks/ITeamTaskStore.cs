﻿using CoreLibrary;
using System.Linq;
using System.Threading.Tasks;

namespace TeamTasks
{
    public interface ITeamTaskStore<TTeamTask> : IAsyncStore<TTeamTask, int>
        where TTeamTask : class, ITeamTask
    {
        Task<IProject> FindProjectByIdAsync(int projectId);
        Task<IAssignment> FindAssignmentAsync(int teamTaskId, int assigneeId);
        Task<ITeamTaskStatus> FindTeamTaskStatusByIdAsync(int teamTaskStatusId);
        Task<ITeamTaskStatus> FindTeamTaskStatusByNameAsync(string teamTaskStatusName);
        Task CreateAssignmentAsync(int teamTaskId, int assignorId, int? assigneeId, string description);
        IQueryable<TTeamTask> GetQueryableTeamTasks();
        IQueryable<IAssignment> GetQueryableAssignments(string teamTaskStatusName);
    }
}
