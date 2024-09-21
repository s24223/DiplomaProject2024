using Domain.Exceptions.UserExceptions;
using Domain.Providers;
using Domain.Templates.Entities;
using Domain.ValueObjects.EntityIdentificators;
using Domain.ValueObjects.PartUrlType;

namespace Domain.Entities.UserPart
{
    public class DomainUrl : Entity<UrlId>
    {
        //Values
        public Uri Url { get; set; } = null!;
        public string? Name { get; set; }
        public string? Description { get; set; }


        //References
        private DomainUser _user = null!;
        public DomainUser User
        {
            get { return _user; }
            set
            {
                if (_user == null && value != null && value.Id == this.Id.UserId)
                {
                    _user = value;
                    _user.AddUrl(this);
                }
            }
        }


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
            //Values with exeptions
            try
            {
                Url = new Uri(url);
            }
            catch (Exception)
            {
                throw new UrlException(Messages.InValidUrl);
            }

            //Values with no exeptions
            Name = name;
            Description = description;
        }

    }
}
