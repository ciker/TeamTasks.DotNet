using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TeamTasks.EntityFramework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using TeamTasks.ResourceServer.Utils;
using TeamTasks.ResourceServer.Provider;
using CoreLibrary;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TeamTasks.ResourceServer.Controllers
{
    [Route("api/[controller]")]
    public class ProjectsController : Controller
    {
        private TeamTasksDbContext db { get; set; }
        private ProjectManager<Project> projectManager;
        private UserManager<TeamTasksUser> userManager;

        public ProjectsController(TeamTasksDbContext context, UserManager<TeamTasksUser> userManager)
        {
            db = context;
            projectManager = new ProjectManager<Project>(new ProjectStore(db));
            this.userManager = userManager;
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> PostAsync([FromBody]SaveProjectViewModel spvm)
        {
            AuthenticatedInfo authInfo = await this.ResolveAuthenticatedEntitiesAsync(db, userManager);

            var res = await projectManager.CreateAsync(spvm, authInfo.UserId, new UserProvider(userManager));

            if (!res.Success)
                return BadRequest(res.Errors);

            return StatusCode(201, new { id = spvm.Id });
        }

        [HttpPut]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> PutAsync([FromBody]SaveProjectViewModel spvm)
        {
            AuthenticatedInfo authInfo = await this.ResolveAuthenticatedEntitiesAsync(db, userManager);

            var res = await projectManager.UpdateAsync(spvm, authInfo.UserId, new UserProvider(userManager));

            if (!res.Success)
                return BadRequest(res.Errors);

            return Ok();
        }

        [HttpDelete]
        [Authorize(Roles = "admin")]
        [Route("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            AuthenticatedInfo authInfo = await this.ResolveAuthenticatedEntitiesAsync(db, userManager);

            var res = await projectManager.DeleteAsync(id, authInfo.UserId, new UserProvider(userManager));

            if (!res.Success)
            {
                if (res.Errors.Contains(ManagerErrors.RecordNotFound))
                    return NotFound(res.Errors);

                return BadRequest(res.Errors);
            }

            return NoContent();
        }
    }
}
