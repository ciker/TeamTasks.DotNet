using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamTasks.EntityFramework
{
    public class Project : IProject
    {
        public string Name { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedDate { get; set; }

        public int CreatorId { get; set; }
        public virtual TeamTasksUser Creator { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? DueDate { get; set; }

        public int Id { get; set; }
    }
}
