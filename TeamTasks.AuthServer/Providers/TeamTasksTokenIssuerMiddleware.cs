using CoreLibrary.AuthServer;
using CoreLibrary.Cryptography;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using TeamTasks.EntityFramework;

namespace TeamTasks.AuthServer.Providers
{
    public class TeamTasksTokenIssuerMiddleware : TokenIssuerMiddlewareBase<TeamTasksAuthServerResponse>
    {
        private UserManager<TeamTasksUser> um;

        public TeamTasksTokenIssuerMiddleware(UserManager<TeamTasksUser> userManager, RequestDelegate next, ICrypter crypter,
            TokenIssuerOptions tokenIssuerOptions) :
            base(next, crypter, tokenIssuerOptions)
        {
            um = userManager;
        }

        protected override void SetOtherAuthServerResponseProperties(AuthServerRequest authRequest, TeamTasksAuthServerResponse authServerResponse)
        {
            TeamTasksUser user = um.FindByNameAsync(authRequest.Username).Result;
            authServerResponse.Roles = um.GetRolesAsync(user).Result.ToList();
        }
    }
}
