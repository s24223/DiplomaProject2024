using Domain.Exceptions.UserExceptions;

namespace Domain.ValueObjects.PartUrlType
{
    public record UrlType
    {
        private static readonly Dictionary<int, UrlType> _types = new();
        //Values
        public UrlTypeEnum Type { get; private set; }
        public string Name { get; private set; } = null!;
        public string Description { get; private set; } = null!;


        //Constructors
        public UrlType(int id)
        {
            if (!_types.TryGetValue(id, out var urlType))
            {
                throw new UrlTypeException(Messages.UndefinedUrlType);
            }
            Type = urlType.Type;
            Name = urlType.Name;
            Description = urlType.Description;
        }

        static UrlType()
        {
            var list = new List<UrlType>();
            list.Add(new UrlType(UrlTypeEnum.Any, "Any", ""));
            list.Add(new UrlType(UrlTypeEnum.GitHubRepository, "GitHubRepository", ""));
            list.Add(new UrlType(UrlTypeEnum.GitHubProject, "GitHubProject", ""));
            list.Add(new UrlType(UrlTypeEnum.LinkedIn, "LinkedIn", ""));
            list.Add(new UrlType(UrlTypeEnum.Portfolio, "Portfolio", ""));
            list.Add(new UrlType(UrlTypeEnum.StackOverflow, "StackOverflow", ""));
            list.Add(new UrlType(UrlTypeEnum.CodePen, "CodePen", ""));
            list.Add(new UrlType(UrlTypeEnum.Blog, "Blog", ""));
            list.Add(new UrlType(UrlTypeEnum.Medium, "Medium", ""));
            list.Add(new UrlType(UrlTypeEnum.Dev_to, "Dev.to", ""));
            list.Add(new UrlType(UrlTypeEnum.YouTube, "YouTube", ""));
            list.Add(new UrlType(UrlTypeEnum.Twitter, "Twitter", ""));

            foreach (var item in list)
            {
                _types.Add((int)item.Type, item);
            }
        }

        private UrlType(UrlTypeEnum type, string name, string description)
        {
            Type = type;
            Name = name;
            Description = description;
        }


        //Methods
        public static Dictionary<int, UrlType> GetTypesDictionary() => _types;
    }
}
