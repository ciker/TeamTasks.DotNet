using CoreLibrary.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace TeamTasks.EntityFramework
{
    public class ProjectStore : EntityStoreBase<Project, int>, IProjectStore<Project>
    {
        public ProjectStore(TeamTasksDbContext context) : base(context)
        {
        }

        public async Task<Project> FindByNameAsync(string name)
        {
            return await db.Set<Project>().SingleOrDefaultAsync(p => p.Name == name);
        }
    }
}
