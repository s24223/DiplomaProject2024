using Domain.Features.Branch.Exceptions.Entities;
using Domain.Features.BranchOffer.Exceptions.Entities;
using Domain.Features.Characteristic.Exceptions;
using Domain.Features.Comment.Exceptions.Entities;
using Domain.Features.Company.Exceptions.Entities;
using Domain.Features.Intership.Exceptions.Entities;
using Domain.Features.Person.Exceptions.Entities;
using Domain.Features.Recruitment.Exceptions.Entities;
using Domain.Features.User.Exceptions.Entities;
using Domain.Shared.Templates.Exceptions;

namespace DomainTests
{
    public class ExceptionTests
    {

        [InlineData(DomainExceptionTypeEnum.BadInputData, "bad input")]
        [InlineData(DomainExceptionTypeEnum.NotFound, "Not Found")]
        [Theory]
        public void BranchException_Theory_Correct(DomainExceptionTypeEnum type, string message)
        {
            var act = new BranchException(message, type);
            Assert.Equal(message, act.Message);
            Assert.Equal(type, act.Type);
        }

        [Fact]
        public void BranchException_CodeNull_Correct()
        {
            var act = new BranchException("message");
            Assert.Equal(DomainExceptionTypeEnum.BadInputData, act.Type);
        }

        [InlineData(DomainExceptionTypeEnum.BadInputData, "bad input")]
        [InlineData(DomainExceptionTypeEnum.NotFound, "Not Found")]
        [Theory]
        public void BranchOfferException_Theory_Correct(DomainExceptionTypeEnum type, string message)
        {
            var act = new BranchOfferException(message, type);
            Assert.Equal(message, act.Message);
            Assert.Equal(type, act.Type);
        }

        [Fact]
        public void BranchOfferException_CodeNull_Correct()
        {
            var act = new BranchOfferException("message");
            Assert.Equal(DomainExceptionTypeEnum.BadInputData, act.Type);
        }

        [InlineData(DomainExceptionTypeEnum.BadInputData, "bad input")]
        [InlineData(DomainExceptionTypeEnum.NotFound, "Not Found")]
        [Theory]
        public void CharacteristicException_Theory_Correct(DomainExceptionTypeEnum type, string message)
        {
            var act = new CharacteristicException(message, type);
            Assert.Equal(message, act.Message);
            Assert.Equal(type, act.Type);
        }

        [Fact]
        public void CharacteristicException_CodeNull_Correct()
        {
            var act = new CharacteristicException("message");
            Assert.Equal(DomainExceptionTypeEnum.BadInputData, act.Type);
        }

        [InlineData(DomainExceptionTypeEnum.BadInputData, "bad input")]
        [InlineData(DomainExceptionTypeEnum.NotFound, "Not Found")]
        [Theory]
        public void CommentException_Theory_Correct(DomainExceptionTypeEnum type, string message)
        {
            var act = new CommentException(message, type);
            Assert.Equal(message, act.Message);
            Assert.Equal(type, act.Type);
        }

        [Fact]
        public void CCommentException_CodeNull_Correct()
        {
            var act = new CommentException("message");
            Assert.Equal(DomainExceptionTypeEnum.BadInputData, act.Type);
        }

        [InlineData(DomainExceptionTypeEnum.BadInputData, "bad input")]
        [InlineData(DomainExceptionTypeEnum.NotFound, "Not Found")]
        [Theory]
        public void CompanyException_Theory_Correct(DomainExceptionTypeEnum type, string message)
        {
            var act = new CompanyException(message, type);
            Assert.Equal(message, act.Message);
            Assert.Equal(type, act.Type);
        }

        [Fact]
        public void CompanyException_CodeNull_Correct()
        {
            var act = new CompanyException("message");
            Assert.Equal(DomainExceptionTypeEnum.BadInputData, act.Type);
        }

        [InlineData(DomainExceptionTypeEnum.BadInputData, "bad input")]
        [InlineData(DomainExceptionTypeEnum.NotFound, "Not Found")]
        [Theory]
        public void IntershipException_Theory_Correct(DomainExceptionTypeEnum type, string message)
        {
            var act = new IntershipException(message, type);
            Assert.Equal(message, act.Message);
            Assert.Equal(type, act.Type);
        }

        [Fact]
        public void IntershipException_CodeNull_Correct()
        {
            var act = new IntershipException("message");
            Assert.Equal(DomainExceptionTypeEnum.BadInputData, act.Type);
        }

        [InlineData(DomainExceptionTypeEnum.BadInputData, "bad input")]
        [InlineData(DomainExceptionTypeEnum.NotFound, "Not Found")]
        [Theory]
        public void PersonException_Theory_Correct(DomainExceptionTypeEnum type, string message)
        {
            var act = new PersonException(message, type);
            Assert.Equal(message, act.Message);
            Assert.Equal(type, act.Type);
        }

        [Fact]
        public void PersonException_CodeNull_Correct()
        {
            var act = new PersonException("message");
            Assert.Equal(DomainExceptionTypeEnum.BadInputData, act.Type);
        }


        [InlineData(DomainExceptionTypeEnum.BadInputData, "bad input")]
        [InlineData(DomainExceptionTypeEnum.NotFound, "Not Found")]
        [Theory]
        public void RecruitmentException_Theory_Correct(DomainExceptionTypeEnum type, string message)
        {
            var act = new RecruitmentException(message, type);
            Assert.Equal(message, act.Message);
            Assert.Equal(type, act.Type);
        }

        [Fact]
        public void RecruitmentException_CodeNull_Correct()
        {
            var act = new RecruitmentException("message");
            Assert.Equal(DomainExceptionTypeEnum.BadInputData, act.Type);
        }

        [InlineData(DomainExceptionTypeEnum.BadInputData, "bad input")]
        [InlineData(DomainExceptionTypeEnum.NotFound, "Not Found")]
        [Theory]
        public void UserException_Theory_Correct(DomainExceptionTypeEnum type, string message)
        {
            var act = new UserException(message, type);
            Assert.Equal(message, act.Message);
            Assert.Equal(type, act.Type);
        }

        [Fact]
        public void UserException_CodeNull_Correct()
        {
            var act = new UserException("message");
            Assert.Equal(DomainExceptionTypeEnum.BadInputData, act.Type);
        }
    }
}
