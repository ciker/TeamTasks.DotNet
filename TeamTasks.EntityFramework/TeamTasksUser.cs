using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace TeamTasks.EntityFramework
{
    public class TeamTasksUser : IdentityUser<int>
    { 
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
