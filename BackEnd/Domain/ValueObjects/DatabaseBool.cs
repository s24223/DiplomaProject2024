namespace Domain.ValueObjects
{
    public record DatabaseBool
    {
        public bool Value { get; private set; }

        public DatabaseBool(string value)
        {
            if (value.ToLower() == "y")
            {
                Value = true;
            }
            else
            {
                Value = false;
            }
        }
    }
}
