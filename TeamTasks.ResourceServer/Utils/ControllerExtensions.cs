using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeamTasks.EntityFramework;

namespace TeamTasks.ResourceServer.Utils
{
    public static class ControllerExtensions
    {
        public static async Task<bool> IsUserAdminAsync(this Controller controller, UserManager<TeamTasksUser> UM)
        {
            if (String.IsNullOrEmpty(controller.User.Identity.Name))
                return false;

            TeamTasksUser user = await UM.FindByNameAsync(controller.User.Identity.Name);

            if (user == null)
                return false;

            return await UM.IsInRoleAsync(user, RoleNames.Administrator);
        }

        public static async Task<AuthenticatedInfo> ResolveAuthenticatedEntitiesAsync(this Controller controller, TeamTasksDbContext context,
            UserManager<TeamTasksUser> userManager)
        {
            string username = controller.Request.HttpContext.User?.Identity?.Name ?? "";
            TeamTasksUser user = await userManager.FindByNameAsync(username);

            int userId = user?.Id ?? 0;

            return new AuthenticatedInfo
            {
                UserId = userId
            };
        }

        public static List<string> ValidateIncomingModel(this Controller controller)
        {
            if (!controller.ModelState.IsValid)
            {
                List<string> errorMessages = new List<string>();
                foreach (var modelState in controller.ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        errorMessages.Add(error.ErrorMessage);
                    }
                }

                return errorMessages;
            }

            else return new List<string>();
        }
    }
}
