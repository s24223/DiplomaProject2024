using Domain.Shared.Templates.ValueObjects.Identificators;

namespace Domain.VerticalSlice.UserProblemPart.ValueObjects.Identificators
{
    public record UserProblemId : GuidId
    {
        public UserProblemId(Guid? value) : base(value)
        {
        }
    }
}
