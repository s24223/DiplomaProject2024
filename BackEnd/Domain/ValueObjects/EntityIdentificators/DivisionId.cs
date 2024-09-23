using Domain.Templates.ValueObjects.EntityIdentificators;

namespace Domain.ValueObjects.EntityIdentificators
{
    public record DivisionId : IntId
    {
        public DivisionId(int value) : base(value)
        {
        }
    }
}
