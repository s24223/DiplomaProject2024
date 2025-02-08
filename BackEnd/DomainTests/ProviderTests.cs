using Domain.Shared.Providers.ExceptionMessage;
using Domain.Shared.Providers.Time;
using Microsoft.Data.SqlClient;
using System.Reflection;
using System.Runtime.Serialization;

namespace DomainTests
{
    public class ProviderTests
    {
        //Properties 
        private readonly ITimeProvider _timeProvider = new Domain.Shared.Providers.Time.TimeProvider();
        private readonly IExceptionMessageProvider _exceptionProvider = new Domain.Shared.Providers.ExceptionMessage.ExceptionMessageProvider();

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
        public void TimeProvider_YearsDifference_Correct(int addYears)
        {
            var date = DateOnly.FromDateTime(DateTime.Today.AddYears(addYears));
            var act = _timeProvider.YearsDifference(date);
            Assert.Equal(0, act);
        }

        [InlineData(true)]
        [InlineData(false)]
        [Theory]
        public void ExceptionMessageProvider_GenerateExceptionMessage_Correct(bool isNullMethod)
        {
            var type = this.GetType();
            var method = isNullMethod ? null : MethodBase.GetCurrentMethod();
            var inputData = "in";
            var message = "ho ho ow";
            var act = _exceptionProvider.GenerateExceptionMessage(
                type, method, inputData, message
                );
            Assert.Contains(inputData, act);
            Assert.Contains(message, act);
        }

        [InlineData(true)]
        [InlineData(false)]
        [Theory]
        public void ExceptionMessageProvider_GenerateExceptionMessageWithException_Correct(bool isNullMethod)
        {
            var type = this.GetType();
            var method = isNullMethod ? null : MethodBase.GetCurrentMethod();
            var ex = new Exception();
            var inputData = "in";
            var message = "ho ho ow";
            var act = _exceptionProvider.GenerateExceptionMessage(
                type, method, ex, inputData, message
                );
            Assert.Contains(inputData, act);
            Assert.Contains(message, act);
            Assert.Contains(ex.GetType().Name, act);
        }


        [InlineData(true)]
        [InlineData(false)]
        [Theory]
        public void ExceptionMessageProvider_GenerateExceptionMessageWithSqlException_Correct(bool isNullMethod)
        {
            var type = this.GetType();
            var method = isNullMethod ? null : MethodBase.GetCurrentMethod();
            SqlException ex = CreateFakeSqlException(2627, "Duplicate key error");

            var inputData = "in";
            var message = "ho ho ow";
            var act = _exceptionProvider.GenerateExceptionMessage(
                type, method, ex, inputData, message
                );

            Assert.Contains(inputData, act);
            Assert.Contains(message, act);
            Assert.Contains(ex.GetType().Name, act);
        }


        //=================================================================================================
        //=================================================================================================
        //=================================================================================================
        //private methods

        private SqlException CreateFakeSqlException(int errorCode, string message)
        {
            var serializationInfo = new SerializationInfo(typeof(SqlException), new FormatterConverter());

            serializationInfo.AddValue("ClassName", typeof(SqlException).FullName);
            serializationInfo.AddValue("Message", message);
            serializationInfo.AddValue("Data", null);
            serializationInfo.AddValue("InnerException", null); // Poprawka!
            serializationInfo.AddValue("HelpURL", null);
            serializationInfo.AddValue("StackTraceString", null);
            serializationInfo.AddValue("RemoteStackTraceString", null);
            serializationInfo.AddValue("RemoteStackIndex", 0);
            serializationInfo.AddValue("ExceptionMethod", null);
            serializationInfo.AddValue("HResult", errorCode);
            serializationInfo.AddValue("Source", "FakeSqlServer");

            var context = new StreamingContext(StreamingContextStates.All);

            var sqlExceptionCtor = typeof(SqlException)
                .GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic,
                                null, new[] { typeof(SerializationInfo), typeof(StreamingContext) }, null);

            if (sqlExceptionCtor == null)
                throw new InvalidOperationException("Constructor for SqlException not found. Check your .NET version.");

            return (SqlException)sqlExceptionCtor.Invoke(new object[] { serializationInfo, context });
        }
    }
}
