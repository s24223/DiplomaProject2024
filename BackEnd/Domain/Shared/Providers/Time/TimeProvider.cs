namespace Domain.Shared.Providers.Time
{
    public class TimeProvider : ITimeProvider
    {
        public DateTime GetDateTimeNow() { return DateTime.Now; }
        public DateTime GetDateTimeToday() { return DateTime.Today; }

        public DateOnly GetDateOnlyToday() { return DateOnly.FromDateTime(GetDateTimeNow()); }

        public DateTime ConvertToDateTime(DateOnly dateOnly) => dateOnly.ToDateTime(TimeOnly.MinValue);
    }
}
