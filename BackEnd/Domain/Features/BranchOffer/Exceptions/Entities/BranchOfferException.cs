using Domain.Shared.Templates.Exceptions;

namespace Domain.Features.BranchOffer.Exceptions.Entities
{
    public class BranchOfferException : DomainException
    {
        public BranchOfferException
            (
            string? message,
            DomainExceptionTypeEnum type = DomainExceptionTypeEnum.BadInputData
            ) : base(message, type)
        {
        }
    }
}
