using Domain.Shared.Templates.Exceptions;

namespace Domain.Features.Person.Exceptions.Entities
{
    public class PersonException : DomainException
    {
        public PersonException
            (
            string? message,
            DomainExceptionTypeEnum type = DomainExceptionTypeEnum.BadInputData
            ) : base(message, type)
        {
        }
    }
}
