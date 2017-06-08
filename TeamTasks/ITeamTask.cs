using CoreLibrary;
using System;

namespace TeamTasks
{
    public interface ITeamTask : IIdentifiable<int>
    {
        /// <summary>
        /// The name of the task. This is NOT a unique field.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// When this record is given
        /// </summary>
        DateTime CreatedDate { get; set; }

        /// <summary>
        /// When the task is in effect
        /// </summary>
        DateTime? StartDate { get; set; }

        /// <summary>
        /// When the task is due
        /// </summary>
        DateTime? DueDate { get; set; }

        /// <summary>
        /// Which project this task belongs to. Nullable means a task can be created
        /// independent of a project.
        /// </summary>
        int? ProjectId { get; set; }

        /// <summary>
        /// Info about the task
        /// </summary>
        string Description { get; set; }
        
        /// <summary>
        /// TeamTask is now a tree hierarchy, but only for display and semantic purposes.
        /// </summary>
        int? ParentTeamTaskId { get; set; }

        /// <summary>
        /// Among its siblings, the importance of a task. Lesser value means higher priority.
        /// It is nullable because a task may be independent of a project or has no siblings.
        /// </summary>
        int? Priority { get; set; }

        /// <summary>
        /// The status of a team task
        /// </summary>
        int TeamTaskStatusId { get; set; }
    }
}
