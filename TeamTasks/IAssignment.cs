using CoreLibrary;
using System;

namespace TeamTasks
{
    /// <summary>
    /// Represents assigning a task to an team member
    /// </summary>
    public interface IAssignment : IIdentifiable<int>
    {
        /// <summary>
        /// Who is assigning
        /// </summary>
        int AssignorId { get; set; }

        /// <summary>
        /// Who is being assigned
        /// </summary>
        int? AssigneeId { get; set; }

        /// <summary>
        /// Which task is being assigned
        /// </summary>
        int TeamTaskId { get; set; }

        /// <summary>
        /// When this record is created
        /// </summary>
        DateTime CreatedDate { get; set; }

        /// <summary>
        /// Info about the assignment
        /// </summary>
        string Description { get; set; }
    }
}
