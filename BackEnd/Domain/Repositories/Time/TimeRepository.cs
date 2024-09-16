namespace Domain.Repositories.Time
{
    public class TimeRepository : ITimeRepository
    {
        public DateTime GetDateTimeNow() { return DateTime.Now; }
        public DateTime GetDateTimeToday() { return DateTime.Today; }

        public DateOnly GetDateOnlyToday() { return DateOnly.FromDateTime(GetDateTimeNow()); }
    }
}
