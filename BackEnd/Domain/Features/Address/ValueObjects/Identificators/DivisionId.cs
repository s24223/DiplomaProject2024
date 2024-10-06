using Domain.Shared.Templates.ValueObjects.Identificators;

namespace Domain.Features.Address.ValueObjects.Identificators
{
    public record DivisionId : IntId
    {
        public DivisionId(int value) : base(value)
        {
        }
    }
}
