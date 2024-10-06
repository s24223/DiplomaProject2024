using Domain.Shared.Templates.ValueObjects.Identificators;

namespace Domain.Features.Branch.ValueObjects.Identificators
{
    public record BranchId : GuidId
    {
        public BranchId(Guid? value) : base(value)
        {
        }
    }
}
