using Domain.Shared.Templates.ValueObjects.Identificators;

namespace Domain.Features.Characteristic.ValueObjects.Identificators
{
    public record CharacteristicId : IntId
    {
        public CharacteristicId(int value) : base(value)
        {
        }
    }
}
