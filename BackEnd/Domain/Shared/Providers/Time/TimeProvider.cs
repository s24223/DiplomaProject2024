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

        public int YearsDifference(DateOnly dateOnly)
        {
            var today = GetDateOnlyToday();
            if (dateOnly >= today)
            {
                return 0;
            }
            var result = new DateOnly().AddDays(today.DayNumber - dateOnly.DayNumber);
            return result.Year - 1;
        }
    }
}
