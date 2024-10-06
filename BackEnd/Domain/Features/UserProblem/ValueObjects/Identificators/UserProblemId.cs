using Domain.Shared.Templates.ValueObjects.Identificators;

namespace Domain.Features.UserProblem.ValueObjects.Identificators
{
    public record UserProblemId : GuidId
    {
        public UserProblemId(Guid? value) : base(value)
        {
        }
    }
}
