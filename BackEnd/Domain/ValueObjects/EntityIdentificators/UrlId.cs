using Domain.ValueObjects.PartUrlType;

namespace Domain.ValueObjects.EntityIdentificators
{
    public record UrlId
    {
        public UserId UserId { get; private set; }
        public UrlType UrlType { get; private set; }
        public DateTime Created { get; private set; }

        public UrlId(UserId userId, UrlType urlType, DateTime created)
        {
            UserId = userId;
            UrlType = urlType;
            Created = created;
        }

    }
}
