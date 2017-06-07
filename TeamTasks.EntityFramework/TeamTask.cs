using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamTasks.EntityFramework
{
    public class TeamTask : ITeamTask
    {
        public string Name { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedDate { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? DueDate { get; set; }

        public int? ProjectId { get; set; }
        public virtual Project Project { get; set; }

        public string Description { get; set; }

        public int Id { get; set; }
    }
}
