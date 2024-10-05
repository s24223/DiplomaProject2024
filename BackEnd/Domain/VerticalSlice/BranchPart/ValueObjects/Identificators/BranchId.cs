using Domain.Shared.Templates.ValueObjects.Identificators;

namespace Domain.VerticalSlice.BranchPart.ValueObjects.Identificators
{
    public record BranchId : GuidId
    {
        public BranchId(Guid? value) : base(value)
        {
        }
    }
}
