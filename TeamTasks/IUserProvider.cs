namespace TeamTasks
{
    /// <summary>
    /// Abstract means to obtain basic user information, not hardcoded to any framework
    /// </summary>
    public interface IUserProvider
    {
        bool UserExists(int userId);
    }
}
