using Domain.Shared.Templates.Exceptions;

namespace Domain.Features.Offer.Exceptions.Entities
{
    public class OfferException : DomainException
    {
        public OfferException
            (
            string? message,
            DomainExceptionTypeEnum type = DomainExceptionTypeEnum.BadInputData
            ) : base(message, type)
        {
        }
    }
}
