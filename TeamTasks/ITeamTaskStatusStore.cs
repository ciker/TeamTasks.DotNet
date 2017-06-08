using CoreLibrary;
using System.Threading.Tasks;

namespace TeamTasks
{
    public interface ITeamTaskStatusStore<TTeamTaskStatus> : IAsyncStore<TTeamTaskStatus, int>
        where TTeamTaskStatus : class, ITeamTaskStatus
    {
        Task<TTeamTaskStatus> FindByNameAsync(string name);
    }
}
