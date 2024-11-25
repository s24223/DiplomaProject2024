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

        //Default Setters
        public DomainUser User
        {
            get { return _user; }
            set
            {
                if (_user == null && value != null && value.Id == Id.UserId)
                {
                    _user = value;
                    _user.AddUrls([this]);
                }
            }
        }

        //Custom Methods
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


        //Static Methods
        public static (
            IEnumerable<DomainUrl> Correct,
            Dictionary<DomainUrl, List<DomainUrl>> Duplicates
                )
            SeparateDuplicates(IEnumerable<DomainUrl> inputs)
        {
            var correct = new Dictionary<string, DomainUrl>();
            var duplicates = new Dictionary<DomainUrl, List<DomainUrl>>();

            foreach (var item in inputs)
            {
                var path = item.Path;
                if (!correct.TryGetValue(item.Path, out var existItem))
                {
                    correct[path] = item;
                }
                else
                {
                    if (!duplicates.TryGetValue(existItem, out var duplicate))
                    {
                        duplicates[existItem] = duplicate = new List<DomainUrl>() { item };
                    }
                    else
                    {
                        duplicate.Add(item);
                    }
                }
            }

            foreach (var item in duplicates.Keys)
            {
                correct.Remove(item.Path);
            }

            return (correct.Values, duplicates);
        }


        public static (
            IEnumerable<DomainUrl> Correct,
            Dictionary<DomainUrl, DomainUrl> Duplicates
            )
            FindConflictsWithDatabase(IEnumerable<DomainUrl> databases, IEnumerable<DomainUrl> inputs)
        {
            var databasesDictionary = databases.ToDictionary(x => x.Path);

            var correct = new List<DomainUrl>();
            var duplicates = new Dictionary<DomainUrl, DomainUrl>();
            foreach (var item in inputs)
            {
                if (!databasesDictionary.TryGetValue(item.Path, out var database))
                {
                    correct.Add(item);
                }
                else
                {
                    duplicates[database] = item;
                }
            }
            return (correct, duplicates);
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
