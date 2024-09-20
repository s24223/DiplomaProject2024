using Domain.ValueObjects.UrlTypePart;

namespace Domain.ValueObjects.EntityIdentificators
{
    public record UrlId
    {
        public UserId UserId { get; private set; }
        public UrlType UrlType { get; private set; }
        public DateTime PublishDate { get; private set; }

        public UrlId(UserId userId, UrlType urlType, DateTime publishDate)
        {
            UserId = userId;
            UrlType = urlType;
            PublishDate = publishDate;
        }

    }
}
