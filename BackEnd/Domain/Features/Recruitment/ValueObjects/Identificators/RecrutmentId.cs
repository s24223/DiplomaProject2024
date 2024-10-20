using Domain.Shared.Templates.ValueObjects.Identificators;

namespace Domain.Features.Recruitment.ValueObjects.Identificators
{
    public record RecrutmentId : GuidId
    {
        public RecrutmentId(Guid? value) : base(value)
        {
        }
    }
}
