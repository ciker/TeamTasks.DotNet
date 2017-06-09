using CoreLibrary.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

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

        public async Task<IProjectStatus> FindProjectStatusByNameAsync(string name)
        {
            return await db.Set<ProjectStatus>().SingleOrDefaultAsync(ps => ps.Name == name);
        }

        public IQueryable<ITeamTask> GetQueryableTeamTasks()
        {
            return db.Set<TeamTask>();
        }
    }
}
