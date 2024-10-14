namespace Domain.Shared.Providers.Time
{
    public interface ITimeProvider
    {
        //Created Part
        DateTime GetDateTimeNow();
        DateTime GetDateTimeToday();

        //Created Part
        DateOnly GetDateOnlyToday();

        //
        DateTime ConvertToDateTime(DateOnly dateOnly);
    }
}
