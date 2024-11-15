using Domain.Shared.Templates.ValueObjects.Identificators;

namespace Domain.Features.Characteristic.ValueObjects.Identificators
{
    public record QualityId : IntId
    {
        public QualityId(int value) : base(value)
        {
        }
    }
}
