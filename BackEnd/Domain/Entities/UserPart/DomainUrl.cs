using Domain.Exceptions.UserExceptions;
using Domain.Providers;
using Domain.Templates.Entities;
using Domain.ValueObjects.EntityIdentificators;
using Domain.ValueObjects.UrlTypePart;

namespace Domain.Entities.UserPart
{
    public class DomainUrl : Entity<UrlId>
    {
        //Values
        public Uri Url { get; set; } = null!;
        public string? Name { get; set; }
        public string? Description { get; set; }

        //References
        public DomainUser User { get; set; } = null!;

        //Constructor
        public DomainUrl
            (
            Guid userId,
            int urlTypeId,
            DateTime publishDate,
            string url,
            string? name,
            string? description,
            IDomainProvider provider
            ) : base(new UrlId
                (
                new UserId(userId),
                new UrlType(urlTypeId),
                publishDate
                ), provider)
        {
            try
            {
                Url = new Uri(url);
            }
            catch (Exception)
            {
                throw new UrlException(Messages.InValidUrl);
            }
            Name = name;
            Description = description;
        }

    }
}
