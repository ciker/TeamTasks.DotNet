using System;
using System.Collections.Generic;
using System.Text;
using TeamTasks.EntityFramework;

namespace TeamTasks.Tests
{
    public static class ProjectStatusTests
    {
        public static void CreateProjectStatus(TeamTasksDbContext db)
        {
            ProjectStatusManager<ProjectStatus> PM = new ProjectStatusManager<ProjectStatus>(new ProjectStatusStore(db));

            var res = PM.CreateAsync(new ProjectStatus
            {
                Name = "Unknown"
            }).Result;

            var dummy = 3;
        }

        public static void UpdateProjectStatus(TeamTasksDbContext db)
        {
            ProjectStatusManager<ProjectStatus> PM = new ProjectStatusManager<ProjectStatus>(new ProjectStatusStore(db));

            ProjectStatus projectStatus = PM.FindByIdAsync(8).Result;
            projectStatus.Name = "dropped";

            var res = PM.UpdateAsync(projectStatus).Result;

            var dummy = 3;
        }
    }
}
