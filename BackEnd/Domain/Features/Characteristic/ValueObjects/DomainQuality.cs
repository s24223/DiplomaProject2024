namespace Domain.Features.Characteristic.ValueObjects
{
    public record DomainQuality
    {
        //Values
        public int Id { get; private set; }
        public string Name { get; private set; } = null!;
        public string Description { get; private set; } = null!;


        //Constructor
        public DomainQuality(int id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }
    }
}
