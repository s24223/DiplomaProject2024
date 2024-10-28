namespace Domain.Shared.Providers.Time
{
    public class TimeProvider : ITimeProvider
    {
        public DateTime GetDateTimeNow() { return DateTime.Now; }
        public DateTime GetDateTimeToday() { return DateTime.Today; }

        public DateOnly GetDateOnlyToday() { return DateOnly.FromDateTime(GetDateTimeNow()); }

        public DateTime ToDateTime(DateOnly dateOnly) => dateOnly.ToDateTime(TimeOnly.MinValue);

        public DateOnly ToDateOnly(DateTime dateTime)
        {
            return DateOnly.FromDateTime(dateTime);
        }
    }
}
