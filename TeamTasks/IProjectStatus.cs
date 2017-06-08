using CoreLibrary;

namespace TeamTasks
{
    public interface IProjectStatus : IIdentifiable<int>
    {
        string Name { get; set; }
    }
}
