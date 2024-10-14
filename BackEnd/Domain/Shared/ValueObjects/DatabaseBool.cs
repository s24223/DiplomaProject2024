using Domain.Shared.Exceptions.ValueObjects;
using Domain.Shared.Templates.Exceptions;

namespace Domain.Shared.ValueObjects
{
    /// <summary>
    /// 
    /// </summary>
    /// <exception cref="DatabaseBoolException"></exception>
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
                throw new DatabaseBoolException
                    (
                    Messages.DatabaseBool_Code_Invalid,
                    DomainExceptionTypeEnum.AppProblem
                    );
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


        //=================================================================================================
        //=================================================================================================
        //=================================================================================================
        //Private methods
    }
}
