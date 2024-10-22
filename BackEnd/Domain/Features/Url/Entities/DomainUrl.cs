using Domain.Features.Url.Exceptions.Entities;
using Domain.Features.Url.Exceptions.ValueObjects;
using Domain.Features.Url.Repository;
using Domain.Features.Url.ValueObjects;
using Domain.Features.Url.ValueObjects.Identificators;
using Domain.Features.User.Entities;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Shared.Providers;
using Domain.Shared.Templates.Entities;

namespace Domain.Features.Url.Entities
{
    public class DomainUrl : Entity<UrlId>
    {
        private readonly IDomainUrlTypeDictionariesRepository _dictionaries;

        //Values
        public string Path { get; private set; } = null!;
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
        public DomainUrlType Type { get; private set; }


        //Constructor
        public DomainUrl
            (
            Guid userId,
            int urlTypeId,
            DateTime created,
            string path,
            string? name,
            string? description,
            IDomainUrlTypeDictionariesRepository dictionaries,
            IProvider provider
            ) : base(new UrlId
                (
                new UserId(userId),
                urlTypeId,
                created
                ), provider)
        {
            _dictionaries = dictionaries;

            Type = GetTypeById(urlTypeId);//With Exception
            SetPath(path); //With Exception

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
            SetPath(path); //With Exception
            Name = name;
            Description = description;
        }


        //==================================================================================================
        //==================================================================================================
        //==================================================================================================
        //Private Methods
        private DomainUrlType GetTypeById
            (
            int id
            )
        {
            if (!_dictionaries.GetDomainUrlTypeDictionary().TryGetValue(id, out var type))
            {
                throw new UrlTypeException($"{Messages.UrlType_Id_NotFound}: {id}");
            }
            return type;
        }

        private void SetPath(string path)
        {
            try
            {
                Path = new Uri(path).ToString();
            }
            catch (Exception)
            {
                throw new UrlException($"{Messages.Url_Path_Invalid}: {path}");
            }
        }
    }
}
