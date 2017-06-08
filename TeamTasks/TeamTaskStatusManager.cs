using CoreLibrary;
using System.Threading.Tasks;

namespace TeamTasks
{
    public class TeamTaskStatusManager<TTeamTaskStatus> : ManagerBase<TTeamTaskStatus, int>
        where TTeamTaskStatus : class, ITeamTaskStatus
    {
        public TeamTaskStatusManager(ITeamTaskStatusStore<TTeamTaskStatus> store) : base(store)
        {
        }

        protected ITeamTaskStatusStore<TTeamTaskStatus> GetTeamTaskStatusStore()
        {
            return (ITeamTaskStatusStore<TTeamTaskStatus>)Store;
        }

        public override async Task<TTeamTaskStatus> FindUniqueAsync(TTeamTaskStatus matchAgainst)
        {
            return await GetTeamTaskStatusStore().FindByNameAsync(matchAgainst.Name);
        }
    }
}
