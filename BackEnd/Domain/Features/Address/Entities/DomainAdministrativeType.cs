namespace Domain.Features.Address.Entities
{
    public class DomainAdministrativeType
    {
        //Values
        public int Id { get; private set; }
        public string Name { get; private set; } = null!;


        //Cosntructor
        public DomainAdministrativeType
            (
            int id,
            string name
            )
        {
            Id = id;
            Name = name;
        }


        //=================================================================================================
        //=================================================================================================
        //=================================================================================================
        //Public Methods
    }
}
