using CoreLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamTasks
{
    public class TeamTaskManager<TTeamTask> : ManagerBase<TTeamTask, int>
        where TTeamTask : class, ITeamTask, new()
    {
        public TeamTaskManager(ITeamTaskStore<TTeamTask> store) : base(store)
        {
        }

        protected ITeamTaskStore<TTeamTask> GetTeamTaskStore()
        {
            return (ITeamTaskStore<TTeamTask>)Store;
        }

        #region Creation

        public override Task<ManagerResult> CreateAsync(TTeamTask entity)
        {
            throw new NotSupportedException();
        }

        public virtual async Task<ManagerResult> CreateAsync(SaveTeamTaskViewModel sttvm, int requestorId, IUserProvider userProvider)
        {
            if (!userProvider.HasRole(requestorId, RoleNames.Administrator))
                return new ManagerResult(ManagerErrors.Unauthorized);

            ITeamTaskStatus teamTaskStatus = await GetTeamTaskStore().FindTeamTaskStatusByNameAsync(
                (string.IsNullOrEmpty(sttvm.TeamTaskStatusName) ? ProjectStatusNames.Inactive : sttvm.TeamTaskStatusName));

            if (teamTaskStatus == null)
                return new ManagerResult(TeamTasksMessages.TeamTaskStatusNotFound);

            TTeamTask newTeamTask = new TTeamTask();
            newTeamTask.Description = sttvm.Description;
            newTeamTask.DueDate = sttvm.DueDate;
            newTeamTask.Name = sttvm.Name;
            newTeamTask.ParentTeamTaskId = sttvm.ParentTeamTaskId;
            newTeamTask.ProjectId = sttvm.ProjectId;
            newTeamTask.StartDate = sttvm.StartDate;
            newTeamTask.TeamTaskStatusId = teamTaskStatus.Id;

            var baseCreateRes = await base.CreateAsync(newTeamTask);

            if (!baseCreateRes.Success)
                return baseCreateRes;

            sttvm.Id = newTeamTask.Id;

            return new ManagerResult();
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

            if (teamTask.ParentTeamTaskId.HasValue)
            {
                // The parent team task must exist.
                TTeamTask parentTeamTask = GetTeamTaskStore().FindByIdAsync(teamTask.ParentTeamTaskId.Value).Result;

                if (parentTeamTask == null)
                    return new ManagerResult(TeamTasksMessages.ParentTeamTaskNotFound);
                               
                if (teamTask.ProjectId.HasValue)
                {
                    /* The incoming team task's project MUST MATCH the parent team task's project!
                     */
                    if (parentTeamTask.ProjectId != teamTask.ProjectId)
                        return new ManagerResult(TeamTasksMessages.InvalidProjectId);
                }
            }

            return new ManagerResult();
        }

        #endregion Creation

        public override ManagerResult OnUpdateLogicCheck(TTeamTask teamTask)
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
            original.ParentTeamTaskId = entityWithNewValues.ParentTeamTaskId;
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

        public virtual TeamTaskTreeViewModel GetTeamTaskTree(TTeamTask teamTask)
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
                ttdvm.Children.Add(GetTeamTaskTree(childTeamTask));
            });

            return ttdvm;
        }

        public virtual TeamTaskTreeViewModel GetTeamTaskTree(int id)
        {
            TTeamTask teamTask = GetTeamTaskStore().FindByIdAsync(id).Result;
            if (teamTask != null)
                return GetTeamTaskTree(teamTask);

            return null;
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
