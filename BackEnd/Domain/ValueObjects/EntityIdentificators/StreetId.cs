using Domain.Templates.ValueObjects.EntityIdentificators;

namespace Domain.ValueObjects.EntityIdentificators
{
    public record StreetId : IntId
    {
        public StreetId(int value) : base(value)
        {
        }
    }
}
