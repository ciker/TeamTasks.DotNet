using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TeamTasks.EntityFramework
{
    public class TeamTasksDbContext : IdentityDbContext<TeamTasksUser, TeamTasksRole, int>
    {
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TeamTask> TeamTasks { get; set; }

        public TeamTasksDbContext() : base()
        {
        }

        public TeamTasksDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
