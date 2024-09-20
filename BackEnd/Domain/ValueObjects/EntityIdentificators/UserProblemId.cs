using Domain.Templates.ValueObjects.EntityIdentificators;

namespace Domain.ValueObjects.EntityIdentificators
{
    public record UserProblemId : GuidId
    {
        public UserProblemId(Guid? value) : base(value)
        {
        }
    }
}
