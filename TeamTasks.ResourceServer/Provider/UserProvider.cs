using Microsoft.AspNetCore.Identity;
using TeamTasks.EntityFramework;

namespace TeamTasks.ResourceServer.Provider
{
    public class UserProvider : IUserProvider
    {
        private UserManager<TeamTasksUser> userManager { get; set; }

        public UserProvider(UserManager<TeamTasksUser> userManager)
        {
            this.userManager = userManager;
        }

        public bool HasRole(int userId, string roleName)
        {
            TeamTasksUser user = userManager.FindByIdAsync(userId.ToString()).Result;
            if (user == null)
                return false;

            return userManager.IsInRoleAsync(user, roleName).Result;
        }

        public bool UserExists(int userId)
        {
            TeamTasksUser user = userManager.FindByIdAsync(userId.ToString()).Result;
            if (user == null)
                return false;

            return true;
        }
    }
}
