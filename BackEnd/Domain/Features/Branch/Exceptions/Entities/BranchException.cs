using Domain.Shared.Templates.Exceptions;

namespace Domain.Features.Branch.Exceptions.Entities
{
    public class BranchException : DomainException
    {
        public BranchException
            (
            string? message,
            DomainExceptionTypeEnum type = DomainExceptionTypeEnum.BadInputData
            ) : base(message, type)
        {
        }
    }
}
