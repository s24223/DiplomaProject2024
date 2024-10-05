using Domain.Shared.Providers;
using Domain.Shared.Templates.Entities;
using Domain.VerticalSlice.UrlPart.Exceptions;
using Domain.VerticalSlice.UrlPart.ValueObjects.Identificators;
using Domain.VerticalSlice.UrlPart.ValueObjects.UrlTypePart;
using Domain.VerticalSlice.UserPart.Entities;
using Domain.VerticalSlice.UserPart.ValueObjects.Identificators;

namespace Domain.VerticalSlice.UrlPart.Entities
{
    public class DomainUrl : Entity<UrlId>
    {
        //Values
        public Uri Path { get; set; } = null!;
        public string? Name { get; set; }
        public string? Description { get; set; }


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

    }
}
