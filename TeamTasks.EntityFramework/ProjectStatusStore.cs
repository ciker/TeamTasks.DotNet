using CoreLibrary.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace TeamTasks.EntityFramework
{
    public class ProjectStatusStore : EntityStoreBase<ProjectStatus, int>, IProjectStatusStore<ProjectStatus>
    {
        public ProjectStatusStore(TeamTasksDbContext context) : base(context)
        {
        }

        public async Task<ProjectStatus> FindByNameAsync(string name)
        {
            return await db.Set<ProjectStatus>().SingleOrDefaultAsync(ps => ps.Name == name);
        }
    }
}
