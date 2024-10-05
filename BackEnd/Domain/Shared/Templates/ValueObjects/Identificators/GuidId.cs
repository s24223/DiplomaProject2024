namespace Domain.Shared.Templates.ValueObjects.Identificators
{
    public record GuidId
    {
        public Guid Value { get; private set; }

        public GuidId(Guid? value)
        {
            if (value != null)
            {
                Value = value.Value;
            }
            else
            {
                Value = Guid.NewGuid();
            }
        }
    }
}
