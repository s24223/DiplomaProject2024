namespace Domain.Features.Notification.ValueObjects
{
    public record DomainNotificationStatus
    {
        //Values
        public int Id { get; private set; }
        public string Name { get; private set; } = null!;


        //Cosntructor
        public DomainNotificationStatus(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
