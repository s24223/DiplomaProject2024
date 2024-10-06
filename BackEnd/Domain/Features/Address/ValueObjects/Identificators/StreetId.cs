using Domain.Shared.Templates.ValueObjects.Identificators;

namespace Domain.Features.Address.ValueObjects.Identificators
{
    public record StreetId : IntId
    {
        public StreetId(int value) : base(value)
        {
        }
    }
}
