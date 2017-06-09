using System;
using System.Collections.Generic;
using System.Text;

namespace TeamTasks
{
    public interface IDateSpanned
    {
        /// <summary>
        /// When the it's is in effect
        /// </summary>
        DateTime? StartDate { get; set; }

        /// <summary>
        /// When the it is due
        /// </summary>
        DateTime? DueDate { get; set; }
    }
}
