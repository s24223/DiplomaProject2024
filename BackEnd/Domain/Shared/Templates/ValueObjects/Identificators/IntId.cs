namespace Domain.Shared.Templates.ValueObjects.Identificators
{
    public record IntId
    {
        public int Value { get; private set; }

        public IntId(int value)
        {
            Value = value;
        }
    }
}
