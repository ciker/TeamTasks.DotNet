using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamTasks.EntityFramework
{
    public class Assignment : IAssignment
    {
        public int AssignorId { get; set; }
        public virtual TeamTasksUser Assignor { get; set; }

        public int? AssigneeId { get; set; }
        public virtual TeamTasksUser Assignee { get; set; }

        public int TeamTaskId { get; set; }
        public virtual TeamTask TeamTask { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedDate { get; set; }

        public string Description { get; set; }

        public int Id { get; set; }
    }
}
