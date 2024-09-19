namespace Domain.Providers.Time
{
    public interface ITimeProvider
    {
        //DateTime Part
        DateTime GetDateTimeNow();
        DateTime GetDateTimeToday();

        //DateTime Part
        DateOnly GetDateOnlyToday();

    }
}
