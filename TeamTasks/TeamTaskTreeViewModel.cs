using System.Collections.Generic;

namespace TeamTasks
{
    /// <summary>
    /// View model intended for displaying the team task hierarchy.
    /// </summary>
    public class TeamTaskTreeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<TeamTaskTreeViewModel> Children { get; set; }
        public string Status { get; set; }

        public TeamTaskTreeViewModel()
        {
        }
    }
}
