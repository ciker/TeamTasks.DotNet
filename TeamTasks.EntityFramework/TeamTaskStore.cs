using CoreLibrary.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace TeamTasks.EntityFramework
{
    public class TeamTaskStore : EntityStoreBase<TeamTask, int>, ITeamTaskStore<TeamTask>
    {
        public TeamTaskStore(TeamTasksDbContext context) : base(context)
        {
        }

        public async Task CreateAssignmentAsync(int teamTaskId, int assignorId, int? assigneeId, string description)
        {
            Assignment assignment = new Assignment
            {
                TeamTaskId = teamTaskId,
                AssignorId = assignorId,
                AssigneeId = assigneeId,
                Description = description
            };

            db.Set<Assignment>().Add(assignment);
            await db.SaveChangesAsync();
        }

        public async Task<IAssignment> FindAssignmentAsync(int teamTaskId, int assigneeId)
        {
            return await db.Set<Assignment>().SingleOrDefaultAsync(a => a.TeamTaskId == teamTaskId &&
                a.AssigneeId.HasValue && a.AssigneeId.Value == assigneeId);
        }

        public async Task<IProject> FindProjectByIdAsync(int projectId)
        {
            return await db.Set<Project>().SingleOrDefaultAsync(p => p.Id == projectId);
        }

        public async Task<ITeamTaskStatus> FindTeamTaskStatusByIdAsync(int teamTaskStatusId)
        {
            return await db.Set<TeamTaskStatus>().SingleOrDefaultAsync(tts => tts.Id == teamTaskStatusId);
        }

        public async Task<ITeamTaskStatus> FindTeamTaskStatusByNameAsync(string teamTaskStatusName)
        {
            return await db.Set<TeamTaskStatus>().SingleOrDefaultAsync(tts => tts.Name == teamTaskStatusName);
        }
        
        public IQueryable<IAssignment> GetQueryableAssignments(string teamTaskStatusName)
        {
            if(!string.IsNullOrEmpty(teamTaskStatusName))
                return db.Set<Assignment>().Where(a => a.TeamTask.TeamTaskStatus.Name == teamTaskStatusName);

            return db.Set<Assignment>();
        }

        public IQueryable<TeamTask> GetQueryableTeamTasks()
        {
            return db.Set<TeamTask>();
        }
    }
}
