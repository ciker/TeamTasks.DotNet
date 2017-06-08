using CoreLibrary;
using System.Threading.Tasks;

namespace TeamTasks
{
    public interface IProjectStatusStore<TProjectStatus> : IAsyncStore<TProjectStatus, int>
        where TProjectStatus : class, IProjectStatus
    {
        Task<TProjectStatus> FindByNameAsync(string name);
    }
}
