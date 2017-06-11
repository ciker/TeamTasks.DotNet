using System;

namespace TeamTasks
{
    public class SaveTeamTaskViewModel
    {
        /// <summary>
        /// This must be nullable for create, but it is required for update.
        /// </summary>
        public int? Id { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? DueDate { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string TeamTaskStatusName { get; set; }

        public int? ParentTeamTaskId { get; set; }

        public int? ProjectId { get; set; }

        /* Note: Priority is NOT saved from up-front when this view model is used.
         */
    }
}
