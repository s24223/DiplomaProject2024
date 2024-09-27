using Domain.Exceptions.UserExceptions.ValueObjectsExceptions;

namespace Domain.ValueObjects
{
    public record DatabaseBool
    {
        public string Code { get; private set; }
        public bool Value { get; private set; }


        //Constructor
        public DatabaseBool(string value)
        {
            if (value.ToLower() == "y")
            {
                Value = true;
                Code = value;
            }
            else if (value.ToLower() == "n")
            {
                Value = false;
                Code = value;
            }
            else
            {
                throw new DatabaseBoolException(Messages.InValidDatabaseBool);
            }
        }

        public DatabaseBool(bool value)
        {
            if (value)
            {
                Value = value;
                Code = "y";
            }
            else
            {
                Value = value;
                Code = "n";
            }
        }
    }
}
