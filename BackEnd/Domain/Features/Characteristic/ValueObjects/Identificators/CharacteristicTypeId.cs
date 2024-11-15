using Domain.Shared.Templates.ValueObjects.Identificators;

namespace Domain.Features.Characteristic.ValueObjects.Identificators
{
    public record CharacteristicTypeId : IntId
    {
        public CharacteristicTypeId(int value) : base(value)
        {
        }
    }
}
