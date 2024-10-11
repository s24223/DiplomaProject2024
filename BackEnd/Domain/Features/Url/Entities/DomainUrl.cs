using Domain.Features.Url.Exceptions.Entities;
using Domain.Features.Url.ValueObjects.Identificators;
using Domain.Features.Url.ValueObjects.UrlTypePart;
using Domain.Features.User.Entities;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Shared.Providers;
using Domain.Shared.Templates.Entities;

namespace Domain.Features.Url.Entities
{
    public class DomainUrl : Entity<UrlId>
    {
        //Values
        public Uri Path { get; private set; } = null!;
        public string? Name { get; private set; }
        public string? Description { get; private set; }


        //References
        private DomainUser _user = null!;
        public DomainUser User
        {
            get { return _user; }
            set
            {
                if (_user == null && value != null && value.Id == Id.UserId)
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
            DateTime created,
            string path,
            string? name,
            string? description,
            IProvider provider
            ) : base(new UrlId
                (
                new UserId(userId),
                new UrlType(urlTypeId),
                created
                ), provider)
        {
            //Values with exeptions
            try
            {
                Path = new Uri(path);
            }
            catch (Exception)
            {
                throw new UrlException(Messages.InValidUrl);
            }

            //Values with no exeptions
            Name = name;
            Description = description;
        }


        //==================================================================================================
        //==================================================================================================
        //==================================================================================================
        //Public Methods
        public void Update
            (
            string path,
            string? name,
            string? description
            )
        {
            try
            {
                Path = new Uri(path);
            }
            catch (Exception)
            {
                throw new UrlException(Messages.InValidUrl);
            }

            //Values with no exeptions
            Name = name;
            Description = description;
        }

        //==================================================================================================
        //==================================================================================================
        //==================================================================================================
        //Private Methods
    }
}
