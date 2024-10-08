namespace Domain.Shared.Templates.Exceptions
{
    public class DomainException : Exception
    {
        //Values
        public DomainExceptionTypeEnum Type { get; private set; }


        //Cosntructor
        public DomainException
            (
            string? message,
            DomainExceptionTypeEnum type = DomainExceptionTypeEnum.BadInputData
            ) : base(message)
        {
            Type = type;
        }


        //==================================================================================================
        //==================================================================================================
        //==================================================================================================
        //Public Methods
    }
}
