using CoreLibrary;
using System.Threading.Tasks;

namespace TeamTasks
{
    public class ProjectManager<TProject> : ManagerBase<TProject, int>
        where TProject : class, IProject
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

        public override ManagerResult OnCreateLogicCheck(TProject project)
        {
            if (project.StartDate.HasValue && project.DueDate.HasValue &&
                project.StartDate.Value > project.DueDate.Value)
                return new ManagerResult(TeamTasksMessages.InvalidStartAndDueDates);

            return new ManagerResult();
        }

        public override ManagerResult OnUpdateLogicCheck(TProject project)
        {
            return OnCreateLogicCheck(project);
        }

        public override ManagerResult OnDeleteLogicCheck(TProject entity)
        {
            /* We should not be able to delete a project that has dependent
             * entities, like tasks.
             */
            return base.OnDeleteLogicCheck(entity);
        }
    }
}
