using CoreLibrary;
using System.Threading.Tasks;

namespace TeamTasks
{
    public class ProjectStatusManager<TProjectStatus> : ManagerBase<TProjectStatus, int>
        where TProjectStatus : class, IProjectStatus
    {
        public ProjectStatusManager(IProjectStatusStore<TProjectStatus> store) : base(store)
        {
        }

        protected IProjectStatusStore<TProjectStatus> GetProjectStatusStore()
        {
            return (IProjectStatusStore<TProjectStatus>)Store;
        }

        public override async Task<TProjectStatus> FindUniqueAsync(TProjectStatus matchAgainst)
        {
            return await GetProjectStatusStore().FindByNameAsync(matchAgainst.Name);
        }

        public override void OnUpdatePropertyValues(TProjectStatus original, TProjectStatus entityWithNewValues)
        {
            original.Name = entityWithNewValues.Name;
        }
    }
}
