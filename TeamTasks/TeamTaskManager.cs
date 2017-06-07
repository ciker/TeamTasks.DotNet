using CoreLibrary;
using System;
using System.Threading.Tasks;

namespace TeamTasks
{
    public class TeamTaskManager<TTeamTask> : ManagerBase<TTeamTask, int>
        where TTeamTask : class, ITeamTask
    {
        public TeamTaskManager(ITeamTaskStore<TTeamTask> store) : base(store)
        {
        }

        protected ITeamTaskStore<TTeamTask> GetTeamTaskStore()
        {
            return (ITeamTaskStore<TTeamTask>)Store;
        }

        public override ManagerResult OnCreateLogicCheck(TTeamTask teamTask)
        {
            if (teamTask.StartDate.HasValue && teamTask.DueDate.HasValue &&
                teamTask.StartDate.Value > teamTask.DueDate.Value)
                return new ManagerResult(TeamTasksMessages.InvalidStartAndDueDates);

            if (teamTask.ProjectId.HasValue)
            {
                IProject project = GetTeamTaskStore().FindProjectByIdAsync(teamTask.ProjectId.Value).Result;

                if (project == null)
                    return new ManagerResult(TeamTasksMessages.ProjectNotFound);
            }

            return new ManagerResult();
        }

        public override ManagerResult OnUpdateLogicCheck(TTeamTask teamTask)
        {
            return OnCreateLogicCheck(teamTask);
        }

        public virtual async Task<ManagerResult> AssignTaskAsync(TTeamTask teamTask, int assignorId, int? assigneeId,
            string description, IUserProvider userProvider)
        {
            if (!userProvider.UserExists(assignorId))
                return new ManagerResult(TeamTasksMessages.AssignorNotFound);
            if (assigneeId.HasValue && !userProvider.UserExists(assigneeId.Value))
                return new ManagerResult(TeamTasksMessages.AssigneeNotFound);

            if (assigneeId.HasValue)
            {
                IAssignment assignment = await GetTeamTaskStore().FindAssignmentAsync(teamTask.Id, assigneeId.Value);

                if (assignment != null)
                    return new ManagerResult(TeamTasksMessages.TaskAlreadyAssignedToAssignee);
            }

            try
            {
                await GetTeamTaskStore().CreateAssignmentAsync(teamTask.Id, assignorId, assigneeId, description);
            }
            catch(Exception e)
            {
                return e.CreateManagerResult();
            }

            return new ManagerResult();
        }
    }
}
