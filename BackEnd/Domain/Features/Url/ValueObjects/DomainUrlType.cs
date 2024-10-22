namespace Domain.Features.Url.ValueObjects
{
    public record DomainUrlType
    {
        //Values
        public int Id { get; private set; }
        public string Name { get; private set; } = null!;
        public string Description { get; private set; } = null!;


        //Cosntructor
        public DomainUrlType(int id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }
    }
}
