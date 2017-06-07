using CoreLibrary;
using System.Threading.Tasks;

namespace TeamTasks
{
    public interface IProjectStore<TProject> : IAsyncStore<TProject, int>
        where TProject : class, IProject
    {
        Task<TProject> FindByNameAsync(string name);
    }
}
