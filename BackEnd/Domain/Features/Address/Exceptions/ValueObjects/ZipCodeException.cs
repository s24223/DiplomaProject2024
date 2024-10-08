using Domain.Shared.Templates.Exceptions;

namespace Domain.Features.Address.Exceptions.ValueObjects
{
    public class ZipCodeException : DomainException
    {
        public ZipCodeException() : base(Messages.InValidZipCode)
        {
        }
    }
}
