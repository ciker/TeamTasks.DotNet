using CoreLibrary.AuthServer;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using TeamTasks.EntityFramework;

namespace TeamTasks.AuthServer.Providers
{
    public class AdditionalClaimsProvider : IAdditionalClaimsProvider
    {
        private UserManager<TeamTasksUser> userManager { get; set; }

        public AdditionalClaimsProvider(UserManager<TeamTasksUser> userManager)
        {
            this.userManager = userManager;
        }

        public List<KeyValuePair<string, string>> GetAdditionalClientClaims(string clientId)
        {
            return null;
        }

        public List<KeyValuePair<string, string>> GetAdditionalUserClaims(string username)
        {
            TeamTasksUser user = userManager.FindByNameAsync(username).Result;
            if (user != null)
            {
                IList<string> roles = userManager.GetRolesAsync(user).Result;

                List<KeyValuePair<string, string>> claims = new List<KeyValuePair<string, string>>();

                // Add roles first
                foreach (string role in roles)
                {
                    claims.Add(new KeyValuePair<string, string>(ClaimTypes.Role, role));
                }

                return claims;
            }

            return null;
        }
    }
}
