using CoreLibrary;
using System;

namespace TeamTasks
{
    public interface IProject : IIdentifiable<int>, IDateSpanned
    {
        /// <summary>
        /// The name of the project. This must be unique across all projects.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Required. When this record was saved to the data store.
        /// </summary>
        DateTime CreatedDate { get; set; }

        /// <summary>
        /// The user id of who created this project
        /// </summary>
        int CreatorId { get; set; }
        
        /// <summary>
        /// The status of a project
        /// </summary>
        int ProjectStatusId { get; set; }
    }
}
