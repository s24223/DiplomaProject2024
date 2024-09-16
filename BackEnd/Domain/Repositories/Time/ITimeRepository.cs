namespace Domain.Repositories.Time
{
    public interface ITimeRepository
    {
        //DateTime Part
        DateTime GetDateTimeNow();
        DateTime GetDateTimeToday();

        //DateTime Part
        DateOnly GetDateOnlyToday();

    }
}
