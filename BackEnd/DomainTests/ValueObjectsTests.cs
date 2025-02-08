using Domain.Features.Address.Exceptions.ValueObjects;
using Domain.Features.Address.ValueObjects;
using Domain.Features.Address.ValueObjects.Identificators;
using Domain.Features.Branch.ValueObjects.Identificators;
using Domain.Features.BranchOffer.ValueObjects.Identificators;
using Domain.Features.Characteristic.ValueObjects.Identificators;
using Domain.Features.Comment.Exceptions.ValueObjects;
using Domain.Features.Comment.ValueObjects;
using Domain.Features.Comment.ValueObjects.CommentTypePart;
using Domain.Features.Comment.ValueObjects.Identificators;
using Domain.Features.Company.Exceptions.ValueObjects;
using Domain.Features.Company.ValueObjects;
using Domain.Features.Intership.Exceptions.ValueObjects;
using Domain.Features.Intership.ValueObjects;
using Domain.Features.Notification.ValueObjects;
using Domain.Features.Notification.ValueObjects.Identificators;
using Domain.Features.Offer.Exceptions.ValueObjects;
using Domain.Features.Offer.ValueObjects;
using Domain.Features.Offer.ValueObjects.Identificators;
using Domain.Features.Person.Exceptions.ValueObjects;
using Domain.Features.Person.ValueObjects;
using Domain.Features.Recruitment.ValueObjects.Identificators;
using Domain.Features.Url.ValueObjects;
using Domain.Features.Url.ValueObjects.Identificators;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Shared.Exceptions.ValueObjects;
using Domain.Shared.ValueObjects;
using Xunit.Abstractions;

namespace DomainTests
{
    public class ValueObjectsTests
    {
        //Properties/Fields
        private readonly ITestOutputHelper _output;


        //Constructor
        public ValueObjectsTests(ITestOutputHelper output)
        {
            _output = output;
        }


        //==================================================================================================
        //==================================================================================================
        //==================================================================================================
        //Tests
        [Fact]
        public void UserId_Guid_Null()
        {
            var arrange = (Guid?)null;
            var act = new UserId(arrange);
            Assert.IsType<Guid>(act.Value);
        }

        [Fact]
        public void UserId_Guid_Guid()
        {
            var arrange = Guid.NewGuid();
            var act = new UserId(arrange);
            Assert.IsType<Guid>(act.Value);
            Assert.Equal(arrange, act.Value);
        }

        [InlineData(1, "a", "a")]
        [InlineData(2, "b", "b")]
        [InlineData(3, "c", "c")]
        [Theory]
        public void DomainUrlType_Correct(int id, string name, string description)
        {
            var act = new DomainUrlType(id, name, description);
            Assert.Equal(id, act.Id);
        }

        [Fact]
        public void UrlId_RandomData_Correct()
        {
            var userId = new UserId(Guid.NewGuid());
            var type = 1;
            var dateTime = DateTime.Now;

            var act = new UrlId(userId, type, dateTime);
            Assert.Equal(userId, act.UserId);
        }

        [Fact]
        public void UrlId_RandomData_ToString()
        {
            var userId = new UserId(Guid.NewGuid());
            var type = 1;
            var dateTime = DateTime.Now;

            var url = new UrlId(userId, type, dateTime);
            var act = $"{url}";
            Assert.Contains(dateTime.ToString(), act);
        }

        [Fact]
        public void RecrutmentId_Guid_Null()
        {
            var arrange = (Guid?)null;
            var act = new RecrutmentId(arrange);
            Assert.IsType<Guid>(act.Value);
        }

        [Fact]
        public void RecrutmentId_Guid_Guid()
        {
            var arrange = Guid.NewGuid();
            var act = new RecrutmentId(arrange);
            Assert.IsType<Guid>(act.Value);
            Assert.Equal(arrange, act.Value);
        }

        [InlineData("123456789")]
        [InlineData("123456788")]
        [InlineData("111111111")]
        [Theory]
        public void PhoneNumber_Theory_Correct(string phoneNumber)
        {
            var act = new PhoneNumber(phoneNumber);
            Assert.Equal(phoneNumber, (string?)act);
        }

        [InlineData("1234567")]
        [InlineData("1234567889")]
        [InlineData("111111111123")]
        [Theory]
        public void PhoneNumber_Theory_PhoneNumberException(string phoneNumber)
        {
            Assert.Throws<PhoneNumberException>(() => new PhoneNumber(phoneNumber));
        }

        [InlineData(1.01)]
        [InlineData(2.0)]
        [InlineData(2)]
        [Theory]
        public void Money_Theory_Correct(decimal value)
        {
            var act = new Money(value);
            Assert.Equal(value, (decimal)act);
        }

        [InlineData(1.001)]
        [InlineData(1.00121)]
        [Theory]
        public void Money_Theory_MoneyException(decimal value)
        {
            Assert.Throws<MoneyException>(() => new Money(value));
        }

        [InlineData(4, 3)]
        [InlineData(2.50, 0.1)]
        [Theory]
        public void Money_TheoryCheckBigger_Correct(decimal value1, decimal value2)
        {
            var act = (Money)value1 > (Money)value2;
            Assert.True(act);
        }

        [InlineData(4, 3)]
        [InlineData(2.50, 0.1)]
        [Theory]
        public void Money_TheoryCheckLower_Correct(decimal value1, decimal value2)
        {
            var act = (Money)value1 < (Money)value2;
            Assert.False(act);
        }

        [InlineData(4, 3)]
        [InlineData(2.50, 0.1)]
        [InlineData(2.50, 2.50)]
        [Theory]
        public void Money_TheoryCheckLowerOrSame_Correct(decimal value1, decimal value2)
        {
            var act = (Money)value2 <= (Money)value1;
            Assert.True(act);
        }

        [InlineData(4, 3)]
        [InlineData(2.50, 0.1)]
        [InlineData(2.50, 2.50)]
        [Theory]
        public void Money_TheoryCheckBiggerOrSame_Correct(decimal value1, decimal value2)
        {
            var act = (Money)value1 >= (Money)value2;
            Assert.True(act);
        }

        [Fact]
        public void OfferId_Guid_Null()
        {
            var arrange = (Guid?)null;
            var act = new OfferId(arrange);
            Assert.IsType<Guid>(act.Value);
        }

        [Fact]
        public void OfferId_Guid_Guid()
        {
            var arrange = Guid.NewGuid();
            var act = new OfferId(arrange);
            Assert.IsType<Guid>(act.Value);
            Assert.Equal(arrange, act.Value);
        }

        [Fact]
        public void NotificationId_Guid_Null()
        {
            var arrange = (Guid?)null;
            var act = new NotificationId(arrange);
            Assert.IsType<Guid>(act.Value);
        }

        [Fact]
        public void NotificationId_Guid_Guid()
        {
            var arrange = Guid.NewGuid();
            var act = new NotificationId(arrange);
            Assert.IsType<Guid>(act.Value);
            Assert.Equal(arrange, act.Value);
        }

        [Fact]
        public void DomainNotificationSender_RandomData_Correct()
        {
            var id = 1;
            var name = "aaa";
            var description = "aaa";

            var act = new DomainNotificationSender(id, name, description);
            Assert.Equal(id, act.Id);
        }

        [Fact]
        public void DomainNotificationStatus_RandomData_Correct()
        {
            var id = 1;
            var name = "aaa";

            var act = new DomainNotificationStatus(id, name);
            Assert.Equal(id, act.Id);
        }

        [InlineData("aaaa")]
        [InlineData("bbbb")]
        [Theory]
        public void ContractNumber_Theory_Correct(string data)
        {
            var act = new ContractNumber(data);
            Assert.Equal(data, act.Value);
        }

        [InlineData(null)]
        [InlineData("")]
        [Theory]
        public void ContractNumber_Theory_ContractNumberException(string? data)
        {
            Assert.Throws<ContractNumberException>(() => new ContractNumber(data));
        }

        [InlineData("123456789")]
        [InlineData("12345678911234")]
        [Theory]
        public void Regon_Theory_Correct(string data)
        {
            var act = (Regon)(data);
            Assert.Equal(data, (string)act);
        }

        [InlineData("12345678")]
        [InlineData("123456789112345")]
        [Theory]
        public void Regon_Theory_RegonException(string data)
        {
            Assert.Throws<RegonException>(() => (Regon)(data));
        }

        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        [Theory]
        public void CommentEvaluation_Theory_Correct(int data)
        {
            var act = new CommentEvaluation(data);
            Assert.Equal(data, act.Value);
        }

        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(6)]
        [Theory]
        public void CommentEvaluation_Theory_CommentEvaluationException(int data)
        {
            Assert.Throws<CommentEvaluationException>(() => new CommentEvaluation(data));
        }

        [Fact]
        public void DomainCommentType_RandomData_Correct()
        {
            var id = 1;
            var name = "a";
            var description = "a";

            var act = new DomainCommentType(id, name, description);
            Assert.Equal(id, act.Id);
        }

        [InlineData(1, 1)]
        [InlineData(1, 7)]
        [InlineData(1, 30)]
        [InlineData(3, 30)]
        [Theory]
        public void CommentId_Seneder1AndRandomData_Correct(int sender, int type)
        {
            var intershipId = new RecrutmentId(Guid.NewGuid());
            var dateTime = DateTime.Now;

            var act = new CommentId(
                intershipId,
                (CommentSenderEnum)sender,
                (CommentTypeEnum)type,
                dateTime);

            Assert.Equal(type, act.CommentTypeId);
        }

        [InlineData(2, 1)]
        [InlineData(2, 7)]
        [Theory]
        public void CommentId_Seneder2AndRandomData_Correct(int sender, int type)
        {
            var intershipId = new RecrutmentId(Guid.NewGuid());
            var dateTime = DateTime.Now;

            var act = new CommentId(
                intershipId,
                (CommentSenderEnum)sender,
                (CommentTypeEnum)type,
                dateTime);

            Assert.Equal(type + 1, act.CommentTypeId);
        }

        [InlineData(2, 2)]
        [InlineData(2, 8)]
        [InlineData(3, 31)]
        [Theory]
        public void CommentId_IncorrectData_Correct(int sender, int type)
        {
            var intershipId = new RecrutmentId(Guid.NewGuid());
            var dateTime = DateTime.Now;

            Assert.Throws<CommentTypeException>(() => new CommentId(
                intershipId,
                (CommentSenderEnum)sender,
                (CommentTypeEnum)type,
                dateTime));
        }


        [Fact]
        public void QualityId_1_Correct()
        {
            var arrange = 1;
            var act = new QualityId(arrange);
            Assert.IsType<int>(act.Value);
            Assert.Equal(arrange, act.Value);
        }

        [Fact]
        public void CharacteristicTypeId_1_Correct()
        {
            var arrange = 1;
            var act = new CharacteristicTypeId(arrange);
            Assert.IsType<int>(act.Value);
            Assert.Equal(arrange, act.Value);
        }

        [Fact]
        public void CharacteristicId_1_Correct()
        {
            var arrange = 1;
            var act = new CharacteristicId(arrange);
            Assert.IsType<int>(act.Value);
            Assert.Equal(arrange, act.Value);
        }


        [Fact]
        public void BranchOfferId_Guid_Null()
        {
            var arrange = (Guid?)null;
            var act = new BranchOfferId(arrange);
            Assert.IsType<Guid>(act.Value);
        }

        [Fact]
        public void BranchOfferId_Guid_Guid()
        {
            var arrange = Guid.NewGuid();
            var act = new BranchOfferId(arrange);
            Assert.IsType<Guid>(act.Value);
            Assert.Equal(arrange, act.Value);
        }

        [Fact]
        public void BranchId_Guid_Null()
        {
            var arrange = (Guid?)null;
            var act = new BranchId(arrange);
            Assert.IsType<Guid>(act.Value);
        }

        [Fact]
        public void BranchId_Guid_Guid()
        {
            var arrange = Guid.NewGuid();
            var act = new BranchId(arrange);
            Assert.IsType<Guid>(act.Value);
            Assert.Equal(arrange, act.Value);
        }

        [Fact]
        public void AddressId_Guid_Null()
        {
            var arrange = (Guid?)null;
            var act = new AddressId(arrange);
            Assert.IsType<Guid>(act.Value);
        }

        [Fact]
        public void AddressId_Guid_Guid()
        {
            var arrange = Guid.NewGuid();
            var act = new AddressId(arrange);
            Assert.IsType<Guid>(act.Value);
            Assert.Equal(arrange, act.Value);
        }

        [Fact]
        public void DivisionId_1_Correct()
        {
            var arrange = 1;
            var act = new DivisionId(arrange);
            Assert.IsType<int>(act.Value);
            Assert.Equal(arrange, act.Value);
        }

        [Fact]
        public void StreetId_1_Correct()
        {
            var arrange = 1;
            var act = new StreetId(arrange);
            Assert.IsType<int>(act.Value);
            Assert.Equal(arrange, act.Value);
        }

        [InlineData("1", "1")]
        [InlineData("12", "12")]
        [InlineData(" 12 ", "12")]
        [Theory]
        public void ApartmentNumber_Theory_Correct(string value, string expected)
        {
            var act = (ApartmentNumber?)value;
            Assert.Equal(expected, (string?)act);
        }

        [InlineData("", null)]
        [InlineData(" ", null)]
        [InlineData(null, null)]
        [Theory]
        public void ApartmentNumber_Theory_Null(string? value, ApartmentNumber? expected)
        {
            var act = (ApartmentNumber?)value;
            Assert.Equal(expected, act);
        }

        [InlineData("a")]
        [InlineData("12a")]
        [InlineData(" a12a ")]
        [Theory]
        public void ApartmentNumber_Theory_ApartmentNumberException(string value)
        {
            Assert.Throws<ApartmentNumberException>(() => (ApartmentNumber?)value);
        }

        [Fact]
        public void ApartmentNumber_IsNull_StringNull()
        {
            var act = (string?)((ApartmentNumber?)null);
            Assert.Null(act);
        }

        [InlineData("1", "1")]
        [InlineData(" 12a ", "12a")]
        [InlineData(" 12 ", "12")]
        [InlineData(" 12a/22 ", "12a/22")]
        [Theory]
        public void BuildingNumber_Theory_Correct(string value, string expected)
        {
            var act = (BuildingNumber)value;
            Assert.Equal(expected, (string)act);
        }

        [InlineData("1aA")]
        [InlineData(" 12a/ ")]
        [InlineData(" 1a2 ")]
        [InlineData(" 12a/22a ")]
        [Theory]
        public void BuildingNumber_Theory_BuildingNumberException(string value)
        {
            Assert.Throws<BuildingNumberException>(() => (BuildingNumber)value);
        }

        [InlineData(" 111-11", "11111")]
        [InlineData(" 12 111 ", "12111")]
        [InlineData(" 12-111 ", "12111")]
        [InlineData(" 12222 ", "12222")]
        [Theory]
        public void ZipCode_Theory_Correct(string value, string expected)
        {
            var act = (ZipCode)value;
            Assert.Equal(expected, (string)act);
        }

        [InlineData(" 1-11")]
        [InlineData(" 12/111 ")]
        [InlineData("  ")]
        [InlineData(" 122222 ")]
        [Theory]
        public void ZipCode_Theory_ZipCodeException(string value)
        {
            Assert.Throws<ZipCodeException>(() => (ZipCode)value);
        }

        [Fact]
        public void DatabaseBool_Null_Null()
        {
            var arrange = (DatabaseBool?)null;
            var act = (bool?)arrange;
            Assert.Null(act);
        }
        [InlineData("Y", true)]
        [InlineData("y", true)]
        [InlineData("N", false)]
        [InlineData("n", false)]
        [Theory]
        public void DatabaseBool_TheoryStringNullable_Correct(string value, bool expected)
        {
            var act = (DatabaseBool?)value;
            Assert.Equal(expected, (bool?)act);
        }

        [InlineData("Y", true)]
        [InlineData("y", true)]
        [InlineData("N", false)]
        [InlineData("n", false)]
        [Theory]
        public void DatabaseBool_TheoryString_Correct(string value, bool expected)
        {
            var act = (DatabaseBool)value;
            Assert.Equal(expected, (bool)act);
        }

        [InlineData("a")]
        [InlineData("B")]
        [InlineData("C")]
        [InlineData("1")]
        [Theory]
        public void DatabaseBool_Theory_DatabaseBoolException(string value)
        {
            Assert.Throws<DatabaseBoolException>(() => (DatabaseBool)value);
        }

        [InlineData(true, "y")]
        [InlineData(true, "Y")]
        [InlineData(false, "n")]
        [InlineData(false, "N")]
        [Theory]
        public void DatabaseBool_TheoryBool_Correct(bool value, string expected)
        {
            var act = (DatabaseBool)value;
            Assert.Equal(expected.ToLower(), (string)act);
        }


        public static IEnumerable<object[]> GetTestData()
        {
            yield return new object[] { new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 1), (int)0 };
            yield return new object[] { new DateOnly(2024, 5, 11), new DateOnly(2024, 5, 10), (int)1 };
            yield return new object[] { new DateOnly(2024, 5, 11), new DateOnly(2024, 5, 12), (int)1 };
        }

        [Theory]
        [MemberData(nameof(GetTestData))]
        public void Duration_RandomData_Correct(DateOnly d1, DateOnly d2, int expected)
        {

            var act = new Duration(d1, d2);
            _output.WriteLine($"Y{act.Years}-M{act.Months}-D{act.Days}");
            Assert.Equal(expected, act.Days);
        }

        [InlineData("s@wp.pl")]
        [InlineData("s2@wp.pl")]
        [InlineData("zzz@gmail.com")]
        [InlineData("as-2.a@gmail.com")]
        [Theory]
        public void Email_Theory_Correct(string value)
        {
            var act = (Email)value;
            Assert.Equal(value, (string)act);
        }

        [InlineData("s@w")]
        [InlineData("s2@wppl")]
        [InlineData("zzz@.com")]
        [InlineData("as-2.a@@gmail.com")]
        [Theory]
        public void Email_Theory_EmailException(string value)
        {
            Assert.Throws<EmailException>(() => (Email)value);
        }

        [Fact]
        public void UrlSegment_Null_Null()
        {
            var arrange = (UrlSegment?)null;
            var act = (string?)arrange;
            Assert.Null(act);
        }

        [InlineData("aaaAADf")]
        [InlineData("s2-wp_pl")]
        [InlineData("zzzgmail-com")]
        [InlineData("as-2-a-gmail_com")]
        [Theory]
        public void UrlSegment_Theory_Correct(string value)
        {
            var act = (UrlSegment?)value;
            Assert.Equal(value, (string?)act);
        }

        [InlineData(" ")]
        [InlineData("")]
        [InlineData("\n")]
        [Theory]
        public void UrlSegment_Theory_Null(string value)
        {
            var act = (UrlSegment?)value;
            Assert.Equal((UrlSegment?)null, act);
        }

        [InlineData("s@w")]
        [InlineData("s2@wppl")]
        [InlineData("zzz@.com")]
        [InlineData("as-2.a@@gmail.com")]
        [Theory]
        public void UrlSegment_Theory_UrlSegmentException(string value)
        {
            Assert.Throws<UrlSegmentException>(() => (UrlSegment?)value);
        }
    }
}
