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
            value = value.Trim();

            if (value.Equals("y", StringComparison.CurrentCultureIgnoreCase))
            {
                Value = true;
                Code = value;
            }
            else if (value.Equals("n", StringComparison.CurrentCultureIgnoreCase))
            {
                Value = false;
                Code = value;
            }
            else
            {
                throw new DatabaseBoolException
                    (
                    $"{Messages.DatabaseBool_Code_Invalid}: {value}",
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
        //Public methods

        public static explicit operator string(DatabaseBool dbBool)
        {
            return dbBool.Code;
        }

        public static explicit operator DatabaseBool(string val)
        {
            return new DatabaseBool(val);
        }


        public static implicit operator DatabaseBool(bool val)
        {
            return new DatabaseBool(val);
        }

        public static implicit operator bool(DatabaseBool val)
        {
            return val.Value;
        }

        public static implicit operator DatabaseBool?(bool? val)
        {
            return val switch
            {
                null => null,
                _ => new DatabaseBool(val.Value),
            };
        }

        public static implicit operator bool?(DatabaseBool? val)
        {
            return val switch
            {
                null => null,
                _ => val.Value,
            };
        }

        //=================================================================================================
        //=================================================================================================
        //=================================================================================================
        //Private methods
    }
}
