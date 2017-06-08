using CoreLibrary;

namespace TeamTasks
{
    public interface ITeamTaskStatus : IIdentifiable<int>
    {
        string Name { get; set; }
    }
}
