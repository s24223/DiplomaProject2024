using Domain.VerticalSlice.CommentPart.Exceptions;

namespace Domain.VerticalSlice.CommentPart.ValueObjects.CommentTypePart
{
    public record CommentType
    {
        private static Dictionary<int, CommentType> _commentTypes = new();


        //Values
        public int Id { get; private set; }
        public CommentTypeEnum Name { get; private set; }
        public string Description { get; private set; }


        //Cosntructors
        public CommentType(int id)
        {
            if (!_commentTypes.TryGetValue(id, out var item))
            {
                throw new CommentTypeException(Messages.InValidCommentType);
            }
            Id = item.Id;
            Name = item.Name;
            Description = item.Description;
        }

        static CommentType()
        {
            var list = new List<CommentType>();
            list.Add(new CommentType(1, CommentTypeEnum.Person, ""));

            foreach (var item in list)
            {
                _commentTypes.Add(item.Id, item);
            }
        }

        private CommentType(int id, CommentTypeEnum name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }


        //Methods
        public static IReadOnlyDictionary<int, CommentType> GetCommentTypes() => _commentTypes;
    }
}
