using Domain.Shared.Providers.Time;

namespace DomainTests
{
    public class ProviderTests
    {
        //Properties 
        private readonly ITimeProvider _timeProvider = new Domain.Shared.Providers.Time.TimeProvider();

        //============================================================================================
        //============================================================================================
        //============================================================================================
        //Tests
        [Fact]
        public void TimeProvider_GetDateTimeToday_Correct()
        {
            //DateTime polandNow = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Central European Standard Time");
            var act = _timeProvider.GetDateTimeToday();
            Assert.Equal(DateTime.Today, act);
        }

        [Fact]
        public void TimeProvider_GetDateTimeNow_Correct()
        {
            DateTime polandNow = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Central European Standard Time");
            var act = _timeProvider.GetDateTimeNow();
            Assert.Equal(polandNow.Date, act.Date);
            Assert.Equal(polandNow.Hour, act.Hour);
            Assert.Equal(polandNow.Minute, act.Minute);
            Assert.Equal(polandNow.Second, act.Second);
        }

        [Fact]
        public void TimeProvider_ToDateTime_Correct()
        {
            var date = DateOnly.FromDateTime(DateTime.UtcNow);
            var expected = DateTime.Today;

            var act = _timeProvider.ToDateTime(date);
            Assert.Equal(expected, act);
        }


        [Fact]
        public void TimeProvider_ToDateOnly_Correct()
        {
            var today = DateTime.Today;
            var expected = DateOnly.FromDateTime(today);

            var act = _timeProvider.ToDateOnly(today);
            Assert.Equal(expected, act);
        }

        [InlineData(2)]
        [InlineData(0)]
        [Theory]
        public void YearsDifference_Correct(int addYears)
        {
            var date = DateOnly.FromDateTime(DateTime.Today.AddYears(addYears));
            var act = _timeProvider.YearsDifference(date);
            Assert.Equal(0, act);
        }
    }
}
