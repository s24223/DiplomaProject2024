using Domain.Features.Intership.Exceptions.ValueObjects;

namespace Domain.Features.Intership.ValueObjects
{
    public record ContractNumber
    {
        //Value
        public string Value { get; private set; }


        //Cosntructor 
        public ContractNumber(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ContractNumberException(Messages.ContractNumber_Value_IsNullOrWhiteSpace);
            }
            Value = value;
        }


        //====================================================================================================
        //====================================================================================================
        //====================================================================================================
        //Private Methods
    }
}
