using CoreLibrary;

namespace TeamTasks
{
    public static class DateSpannedHelper
    {
        public static ManagerResult CheckDates(this IDateSpanned dateSpanned)
        {
            if (dateSpanned.StartDate.HasValue && dateSpanned.DueDate.HasValue &&
               dateSpanned.StartDate.Value > dateSpanned.DueDate.Value)
                return new ManagerResult(TeamTasksMessages.InvalidStartAndDueDates);

            return new ManagerResult();
        }
    }
}
