namespace Domain.Features.Notification.ValueObjects
{
    public record DomainNotificationSender
    {
        //Values
        public int Id { get; private set; }
        public string Name { get; private set; } = null!;
        public string Description { get; private set; } = null!;


        //Cosntructor
        public DomainNotificationSender(int id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }
    }
}
