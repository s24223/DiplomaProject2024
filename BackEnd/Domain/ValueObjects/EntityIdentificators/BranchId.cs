using Domain.Templates.ValueObjects.EntityIdentificators;

namespace Domain.ValueObjects.EntityIdentificators
{
    public record BranchId : GuidId
    {
        public BranchId(Guid? value) : base(value)
        {
        }
    }
}
