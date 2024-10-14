using Domain.Shared.Templates.Exceptions;

namespace Domain.Features.Intership.Exceptions.ValueObjects
{
    public class ContractNumberException : DomainException
    {
        public ContractNumberException
            (
            string? message,
            DomainExceptionTypeEnum type = DomainExceptionTypeEnum.BadInputData
            ) : base(message, type)
        {
        }
    }
}
