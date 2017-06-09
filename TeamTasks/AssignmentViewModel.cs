using System;

namespace TeamTasks
{
    /// <summary>
    /// View model intended to be seen by a user when they pull up the tasks they're responsible for.
    /// 
    /// </summary>
    public class AssignmentViewModel
    {
        public int Id { get; set; }
        public string TeamTaskName { get; set; }
        public string TeamTaskDescription { get; set; }
        public string TeamTaskStatus { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }        
    }
}
