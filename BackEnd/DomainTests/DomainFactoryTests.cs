using Domain.Shared.Factories;
using Domain.Shared.Providers;
using Domain.Shared.Providers.ExceptionMessage;
using DomainTests.Fakes;
using Xunit.Abstractions;

namespace DomainTests
{
    public class DomainFactoryTests
    {
        //Properties
        private readonly ITestOutputHelper _output;
        private readonly DomainFactory _domainFactory;


        //Constructor
        public DomainFactoryTests(ITestOutputHelper output)
        {
            _output = output;
            _domainFactory = new DomainFactory(
                new Provider(
                    new ExceptionMessageProvider(),
                    new Domain.Shared.Providers.Time.TimeProvider()),
                new DomainNotificationsFake(),
                new DomainUrlFake(),
                new CharacteristicsFake(),
                new CommentTypeFake());
        }


        //====================================================================================
        //====================================================================================
        //====================================================================================
        //Tests

        [Fact]
        public void CreateDomainUser_LoginNull_Exception()
        {
            Assert.Throws<Exception>(() => _domainFactory.CreateDomainUser(
                Guid.NewGuid(),
                null,
                DateTime.Now,
                DateTime.Now));
        }

        [Fact]
        public void CreateDomainUrl_Correct()
        {
            var path = "https://www.youtube.com/";
            var url = _domainFactory.CreateDomainUrl(
                Guid.NewGuid(),
                1,
                path,
                "name",
                null);
            Assert.Equal(path, url.Path);
        }

        [Fact]
        public void CreateDomainPerson_Correct()
        {
            var person = _domainFactory.CreateDomainPerson(
                Guid.NewGuid(),
                null,
                new DateOnly(2000, 1, 1),
                "a@gmail.com",
                "a",
                "a",
                null,
                null,
                null,
                "Y",
                "Y",
                null);
            Assert.Equal("a@gmail.com", person.ContactEmail);
            Assert.Equal(null, person.BirthDate);
        }

        [Fact]
        public void CreateDomainCompany_Correct()
        {
            var company = _domainFactory.CreateDomainCompany(
                Guid.NewGuid(),
                null,
                "a@gmail.com",
                "name",
                "123456789",
                null,
                new DateOnly(2000, 1, 1));
            Assert.Equal(null, company.Description);
            Assert.Equal(null, company.UrlSegment);
        }

        [Fact]
        public void CreateDomainBranch_Correct()
        {
            var branch = _domainFactory.CreateDomainBranch(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                "a",
                "a",
                null);
            Assert.Equal(null, branch.Description);
            Assert.Equal("a", branch.Name);

        }

        [Fact]
        public void CreateDomainOffer_WithoutId_Correct()
        {
            var offer = _domainFactory.CreateDomainOffer(
                "name",
                "name",
                10,
                10,
                true,
                true);
            Assert.Equal(true, offer.IsNegotiatedSalary.Value);
            Assert.Equal(10, offer.MinSalary);
            Assert.Equal(10, offer.MaxSalary);
        }

        [Fact]
        public void CreateDomainOffer_WithIdAndBooleans_Correct()
        {
            var offer = _domainFactory.CreateDomainOffer(
                Guid.NewGuid(),
                "name",
                "name",
                10,
                10,
                true,
                true);
            Assert.Equal(true, offer.IsNegotiatedSalary.Value);
            Assert.Equal(10, offer.MinSalary);
            Assert.Equal(10, offer.MaxSalary);
        }

        [Fact]
        public void CreateDomainOffer_WithIdAndStrings_Correct()
        {
            var offer = _domainFactory.CreateDomainOffer(
                Guid.NewGuid(),
                "name",
                "name",
                10,
                10,
                "Y",
                "Y");
            Assert.Equal(true, offer.IsNegotiatedSalary.Value);
            Assert.Equal(10, offer.MinSalary);
            Assert.Equal(10, offer.MaxSalary);
        }

        [Fact]
        public void CreateDomainBranchOffer_Correct()
        {
            var bo = _domainFactory.CreateDomainBranchOffer(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                DateTime.Now,
                DateTime.Now,
                DateTime.Now,
                null,
                null,
                DateTime.Now);

            Assert.Equal(null, bo.WorkStart);
            Assert.Equal(null, bo.WorkEnd);
        }

        [Fact]
        public void CreateDomainRecruitment_Correct()
        {
            var recruitment = _domainFactory.CreateDomainRecruitment(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                DateTime.Now,
                "Message",
                "Response",
                "Y");
            Assert.Equal(true, recruitment.IsAccepted.Value);
            Assert.Equal("Message", recruitment.PersonMessage);
        }

        [Fact]
        public void CreateDomainIntership_Correct()
        {
            var internship = _domainFactory.CreateDomainIntership(
                Guid.NewGuid(),
                DateTime.Now,
                new DateOnly(2000, 1, 1),
                null,
                "a");
            Assert.Equal(null, internship.ContractEndDate);
            Assert.Equal("a", internship.ContractNumber.Value);
        }
    }
}
