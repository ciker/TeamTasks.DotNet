using CoreLibrary;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace TeamTasks
{
    public class ProjectManager<TProject> : ManagerBase<TProject, int>
        where TProject : class, IProject, new()
    {
        public ProjectManager(IProjectStore<TProject> store) : base(store)
        {
        }

        public IProjectStore<TProject> GetProjectStore()
        {
            return (IProjectStore<TProject>)Store;
        }

        public override async Task<TProject> FindUniqueAsync(TProject matchAgainst)
        {
            return await GetProjectStore().FindByNameAsync(matchAgainst.Name);
        }

        #region Creation
        public override ManagerResult OnCreateLogicCheck(TProject project)
        {           
            var checkDatesRes = project.CheckDates();

            if (!checkDatesRes.Success)
                return checkDatesRes;

            return new ManagerResult();
        }

        public override Task<ManagerResult> CreateAsync(TProject entity)
        {
            throw new NotSupportedException();
        }

        public virtual async Task<ManagerResult> CreateAsync(SaveProjectViewModel spvm, int requestorId, IUserProvider userProvider)
        {
            if (!userProvider.HasRole(requestorId, RoleNames.Administrator))
                return new ManagerResult(ManagerErrors.Unauthorized);

            IProjectStatus projectStatus = await GetProjectStore().FindProjectStatusByNameAsync(spvm.ProjectStatusName);

            if (projectStatus == null)
                return new ManagerResult(TeamTasksMessages.ProjectStatusNotFound);

            TProject newProject = new TProject();
            newProject.CreatorId = requestorId;
            newProject.DueDate = spvm.DueDate;
            newProject.Name = spvm.Name;
            newProject.ProjectStatusId = projectStatus.Id;
            newProject.StartDate = spvm.StartDate;

            return await base.CreateAsync(newProject);
        }

        #endregion Creation

        #region Update
        public override ManagerResult OnUpdateLogicCheck(TProject project)
        {
            return OnCreateLogicCheck(project);
        }

        public override Task<ManagerResult> UpdateAsync(TProject entity)
        {
            throw new NotSupportedException();
        }

        public virtual async Task<ManagerResult> UpdateAsync(SaveProjectViewModel spvm, int requestorId, IUserProvider userProvider)
        {
            if (!userProvider.HasRole(requestorId, RoleNames.Administrator))
                return new ManagerResult(ManagerErrors.Unauthorized);

            if (string.IsNullOrEmpty(spvm.ProjectStatusName))
                return new ManagerResult(TeamTasksMessages.ProjectStatusNotSpecified);

            IProjectStatus projectStatus = await GetProjectStore().FindProjectStatusByNameAsync(spvm.ProjectStatusName);

            if (projectStatus == null)
                return new ManagerResult(TeamTasksMessages.ProjectStatusNotFound);

            if (!spvm.Id.HasValue)
                return new ManagerResult(ManagerErrors.IdNotSpecified);

            TProject project = await FindByIdAsync(spvm.Id.Value);

            if (project == null)
                return new ManagerResult(ManagerErrors.RecordNotFound);

            return await base.UpdateAsync(project);
        }

        public override void OnUpdatePropertyValues(TProject original, TProject entityWithNewValues)
        {
            original.DueDate = entityWithNewValues.DueDate;
            original.Name = entityWithNewValues.Name;
            original.ProjectStatusId = entityWithNewValues.ProjectStatusId;
            original.StartDate = entityWithNewValues.StartDate;
        }

        #endregion Update

        #region Delete
        public override ManagerResult OnDeleteLogicCheck(TProject entity)
        {
            /* We should not be able to delete a project that has dependent
             * entities, like tasks.
             */
            if (GetProjectStore().GetQueryableTeamTasks().Where(p => p.ProjectId.HasValue && p.ProjectId.Value == entity.Id).Any())
                return new ManagerResult(TeamTasksMessages.ProjectHasDependentTeamTasks);

            return new ManagerResult();
        }

        public override Task<ManagerResult> DeleteAsync(int id)
        {
            throw new NotSupportedException();
        }

        public override Task<ManagerResult> DeleteAsync(TProject entity)
        {
            throw new NotSupportedException();
        }

        public virtual async Task<ManagerResult> DeleteAsync(int id, int requestorId, IUserProvider userProvider)
        {
            if (!userProvider.HasRole(requestorId, RoleNames.Administrator))
                return new ManagerResult(ManagerErrors.Unauthorized);

            return await base.DeleteAsync(id);
        }

        public virtual async Task<ManagerResult> DeleteAsync(TProject project, int requestorId, IUserProvider userProvider)
        {
            if (!userProvider.HasRole(requestorId, RoleNames.Administrator))
                return new ManagerResult(ManagerErrors.Unauthorized);

            return await base.DeleteAsync(project);
        }

        #endregion Delete
    }
}
