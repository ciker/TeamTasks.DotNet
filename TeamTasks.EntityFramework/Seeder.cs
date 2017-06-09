using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamTasks.EntityFramework
{
    public class Seeder
    {
        private TeamTasksDbContext db { get; set; }

        private UserManager<TeamTasksUser> UM { get; set; }

        private RoleManager<TeamTasksRole> RM { get; set; }

        public Seeder(TeamTasksDbContext context, UserManager<TeamTasksUser> userManager,
            RoleManager<TeamTasksRole> roleManager)
        {
            db = context;
            UM = userManager;
            RM = roleManager;
        }

        public async Task CreateRolesAsync()
        {
            if (!(await RM.RoleExistsAsync("admin")))
                await RM.CreateAsync(new TeamTasksRole { Name = "admin" });

            if (!(await RM.RoleExistsAsync("member")))
                await RM.CreateAsync(new TeamTasksRole { Name = "member" });
        }

        private async Task CreateMemberUserAsync(string username, params string[] roleNames)
        {
            TeamTasksUser user = await UM.FindByNameAsync(username);

            if (user == null)
            {
                user = new TeamTasksUser
                {
                    UserName = username,
                    Email = $"{username}@teamtasks.net",
                    EmailConfirmed = true
                };
                var res = await UM.CreateAsync(user, "Aaa000$");
                if (res.Succeeded)
                {
                    await UM.AddToRoleAsync(user, "member");
                }
            }
        }

        public async Task CreateUsersAsync()
        {
            // The Admin User
            TeamTasksUser admin = await UM.FindByNameAsync("admin");

            if (admin == null)
            {
                admin = new TeamTasksUser
                {
                    UserName = "admin",
                    Email = "webmaster@teamtasks.net",
                    EmailConfirmed = true
                };
                var res = await UM.CreateAsync(admin, "Aaa000$");
                if (res.Succeeded)
                {
                    await UM.AddToRoleAsync(admin, "member");
                    await UM.AddToRoleAsync(admin, "admin");
                }
            }
            
            await CreateMemberUserAsync("user1");
            await CreateMemberUserAsync("user2");
            await CreateMemberUserAsync("user3");
            await CreateMemberUserAsync("user4");
            await CreateMemberUserAsync("user5");
        }

        public async Task InitializeDataAsync()
        {
            await CreateRolesAsync();
            await CreateUsersAsync();

            List<ProjectStatus> projectStatuses = new List<ProjectStatus>
            {
                new ProjectStatus { Name = ProjectStatusNames.Active },
                new ProjectStatus { Name = ProjectStatusNames.Complete },
                new ProjectStatus { Name = ProjectStatusNames.Inactive }
            };

            projectStatuses.ForEach(ps =>
            {
                AddOrUpdateProjectStatus(ps);
            });

            List<TeamTaskStatus> teamTaskStatuses = new List<TeamTaskStatus>
            {
                new TeamTaskStatus { Name = TeamTaskStatusNames.Active },
                new TeamTaskStatus { Name = TeamTaskStatusNames.Inactive }
            };

            teamTaskStatuses.ForEach(tts =>
            {
                AddOrUpdateTeamTaskStatus(tts);
            });
        }

        private void AddOrUpdateProjectStatus(ProjectStatus projectStatus)
        {
            ProjectStatus p = db.ProjectStatuses.SingleOrDefault(ps => ps.Name == projectStatus.Name);

            if (p == null)
            {
                db.ProjectStatuses.Add(projectStatus);
            }
            else
            {
                p.Name = projectStatus.Name;
            }

            db.SaveChanges();
        }

        private void AddOrUpdateTeamTaskStatus(TeamTaskStatus teamTaskStatus)
        {
            TeamTaskStatus t = db.TeamTaskStatuses.SingleOrDefault(tts => tts.Name == teamTaskStatus.Name);

            if (t == null)
            {
                db.TeamTaskStatuses.Add(teamTaskStatus);
            }
            else
            {
                t.Name = teamTaskStatus.Name;
            }

            db.SaveChanges();
        }
    }
}
