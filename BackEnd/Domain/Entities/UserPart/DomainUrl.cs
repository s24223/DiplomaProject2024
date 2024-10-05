using Domain.Exceptions.UserExceptions.EntitiesExceptions;
using Domain.Providers;
using Domain.Templates.Entities;
using Domain.ValueObjects.EntityIdentificators;
using Domain.ValueObjects.PartUrlType;

namespace Domain.Entities.UserPart
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
            DateTime created,
            string path,
            string? name,
            string? description,
            IDomainProvider provider
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
