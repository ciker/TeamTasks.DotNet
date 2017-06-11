using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TeamTasks.EntityFramework;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using TeamTasks.ResourceServer.Utils;
using TeamTasks.ResourceServer.Provider;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TeamTasks.ResourceServer.Controllers
{
    [Route("api/[controller]")]
    public class TeamTasksController : Controller
    {
        private TeamTasksDbContext db { get; set; }
        private TeamTaskManager<TeamTask> teamTaskManager { get; set; }
        private UserManager<TeamTasksUser> userManager { get; set; }

        public TeamTasksController(TeamTasksDbContext context, UserManager<TeamTasksUser> userManager)
        {
            db = context;
            teamTaskManager = new TeamTaskManager<TeamTask>(new TeamTaskStore(db));
            this.userManager = userManager;
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> PostAsync([FromBody]SaveTeamTaskViewModel sttvm)
        {
            AuthenticatedInfo authInfo = await this.ResolveAuthenticatedEntitiesAsync(db, userManager);

            var res = await teamTaskManager.CreateAsync(sttvm, authInfo.UserId, new UserProvider(userManager));

            if (!res.Success)
                return BadRequest(res.Errors);

            return StatusCode(201, new { id = sttvm.Id });
        }
    }
}
