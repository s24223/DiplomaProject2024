using Domain.Shared.Templates.ValueObjects.Identificators;

namespace Domain.Features.User.ValueObjects.Identificators
{
    public record UserId : GuidId
    {
        public UserId(Guid? value) : base(value)
        {
        }
    }
}
