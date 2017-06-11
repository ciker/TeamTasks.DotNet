using CoreLibrary.AuthServer;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using TeamTasks.EntityFramework;

namespace TeamTasks.AuthServer.Providers
{
    public class SimpleCredentialsProvider : ICredentialsProvider
    {
        private UserManager<TeamTasksUser> _userManager;

        public SimpleCredentialsProvider(UserManager<TeamTasksUser> userManager)
        {
            _userManager = userManager;
        }

        public Task<bool> AreClientCredentialsValidAsync(string clientId, string secret)
        {
            throw new NotSupportedException();
        }

        public async Task<bool> AreUserCredentialsValidAsync(string username, string password)
        {
            TeamTasksUser user = await _userManager.FindByNameAsync(username);
            if (user == null)
                return false;

            return await _userManager.CheckPasswordAsync(user, password);
        }

    }
}
