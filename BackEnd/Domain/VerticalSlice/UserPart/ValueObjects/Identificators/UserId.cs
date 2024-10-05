using Domain.Shared.Templates.ValueObjects.Identificators;

namespace Domain.VerticalSlice.UserPart.ValueObjects.Identificators
{
    public record UserId : GuidId
    {
        public UserId(Guid? value) : base(value)
        {
        }
    }
}
