using Domain.ValueObjects.EntityIdentificators.Templates;

namespace Domain.ValueObjects.EntityIdentificators
{
    public record UserId : GuidId
    {
        public UserId(Guid? value) : base(value)
        {
        }
    }
}
