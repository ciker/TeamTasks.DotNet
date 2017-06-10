using CoreLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var checkDateRes = teamTask.CheckDates();
            if (!checkDateRes.Success)
                return checkDateRes;

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
            /* We shall assume that the teamTask.ParentTeamTaskId can be different from the original's.
             * We just need to make sure that ParentTeamTaskId is NOT one of its descendant's IDs!
             */
            if (teamTask.ParentTeamTaskId.HasValue)
            {
                List<TTeamTask> descendants = new List<TTeamTask>();

                GetDescendants(teamTask, descendants);

                if (descendants.Any(tt => tt.Id == teamTask.ParentTeamTaskId))
                    return new ManagerResult(TeamTasksMessages.ParentTeamTaskCannotBeDescendant);
            }

            return new ManagerResult();
        }

        public override void OnUpdatePropertyValues(TTeamTask original, TTeamTask entityWithNewValues)
        {
            original.Description = entityWithNewValues.Description;
            original.DueDate = entityWithNewValues.DueDate;
            original.Name = entityWithNewValues.Name;
            if (!original.ProjectId.HasValue && entityWithNewValues.ProjectId.HasValue)
                original.ProjectId = entityWithNewValues.ProjectId;
            original.StartDate = entityWithNewValues.StartDate;
            original.TeamTaskStatusId = entityWithNewValues.TeamTaskStatusId;
        }

        protected void GetDescendants(TTeamTask teamTask, List<TTeamTask> descendants)
        {
            List<TTeamTask> immediateChildren = GetTeamTaskStore().GetQueryableTeamTasks().Where(tt => tt.ParentTeamTaskId == teamTask.Id).ToList();
            descendants.AddRange(immediateChildren);

            immediateChildren.ForEach(childTeamTask =>
            {
                GetDescendants(childTeamTask, descendants);
            });
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

        public virtual TeamTaskTreeViewModel GetTeamTaskTreeAsync(TTeamTask teamTask)
        {
            TeamTaskTreeViewModel ttdvm = new TeamTaskTreeViewModel()
            {
                Id = teamTask.Id,
                Name = teamTask.Name,
                Children = new List<TeamTaskTreeViewModel>(),
                Status = GetTeamTaskStore().FindTeamTaskStatusByIdAsync(teamTask.TeamTaskStatusId).Result.Name
            };

            List<TTeamTask> immediateChildren = GetTeamTaskStore().GetQueryableTeamTasks()
                .Where(tt => tt.ParentTeamTaskId == teamTask.Id)
                .OrderBy(tt => tt.Priority).ToList();

            immediateChildren.ForEach(childTeamTask =>
            {
                ttdvm.Children.Add(GetTeamTaskTreeAsync(childTeamTask));
            });

            return ttdvm;
        }

        public virtual ResultSet<AssignmentViewModel> GetAssignments(int assigneeId, string teamTaskStatusName, int page = 1, int pageSize = 10)
        {
            IQueryable<IAssignment> qAssignments = GetTeamTaskStore().GetQueryableAssignments(teamTaskStatusName)
                .Where(a => a.AssigneeId.HasValue && a.AssigneeId.Value == assigneeId);

            ResultSet<IAssignment> assignmentsRS = ResultSetHelper.GetResults<IAssignment, int>(qAssignments, page, pageSize);

            return ResultSetHelper.Convert(assignmentsRS, assignment =>
            {
                TTeamTask teamTask = GetTeamTaskStore().FindByIdAsync(assignment.TeamTaskId).Result;

                return new AssignmentViewModel
                {
                    Id = assignment.Id,
                    Description = assignment.Description,
                    DueDate = teamTask?.DueDate,
                    StartDate = teamTask?.StartDate,
                    TeamTaskDescription = teamTask?.Description ?? "",
                    TeamTaskName = teamTask?.Name ?? "",
                    TeamTaskStatus = GetTeamTaskStore().FindTeamTaskStatusByIdAsync(teamTask.TeamTaskStatusId).Result?.Name ?? ""
                };
            });                        
        }

    }
}
