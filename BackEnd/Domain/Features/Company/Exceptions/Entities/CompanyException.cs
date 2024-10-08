using Domain.Shared.Templates.Exceptions;

namespace Domain.Features.Company.Exceptions.Entities
{
    public class CompanyException : DomainException
    {
        public CompanyException
            (
            string? message,
            DomainExceptionTypeEnum type = DomainExceptionTypeEnum.BadInputData
            ) : base(message, type)
        {
        }
    }
}
