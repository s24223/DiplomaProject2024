using Domain.Shared.Templates.Exceptions;

namespace Domain.Features.Address.Exceptions.ValueObjects
{
    public class BuildingNumberException : DomainException
    {
        public BuildingNumberException
            (
            string? message,
            DomainExceptionTypeEnum type = DomainExceptionTypeEnum.BadInputData
            ) : base(message, type)
        {
        }
    }
}
