using System;

namespace TeamTasks
{
    /// <summary>
    /// View model to use for creating and updating projects
    /// </summary>
    public class SaveProjectViewModel
    {
        /// <summary>
        /// This must be nullable for create, but it is required for update.
        /// </summary>
        public int? Id { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? DueDate { get; set; }

        public string Name { get; set; }

        public string ProjectStatusName { get; set; }
    }
}
