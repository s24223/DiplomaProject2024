using Domain.Features.User.ValueObjects.Identificators;

namespace Domain.Features.Url.ValueObjects.Identificators
{
    public record UrlId
    {
        public UserId UserId { get; private set; }
        public int UrlTypeId { get; private set; }
        public DateTime Created { get; private set; }

        public UrlId(UserId userId, int urlTypeId, DateTime created)
        {
            UserId = userId;
            UrlTypeId = urlTypeId;
            Created = created;
        }


        public override string ToString()
        {
            return $"UserId: {UserId}, UrlTypeId: {UrlTypeId}, Created: {Created}";
        }
    }
}
