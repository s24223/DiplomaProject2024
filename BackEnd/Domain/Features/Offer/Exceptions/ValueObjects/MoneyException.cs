using Domain.Shared.Templates.Exceptions;

namespace Domain.Features.Offer.Exceptions.ValueObjects
{
    public class MoneyException : DomainException
    {
        public MoneyException
            (
            string? message,
            DomainExceptionTypeEnum type = DomainExceptionTypeEnum.BadInputData
            ) : base(message, type)
        {
        }
    }
}
