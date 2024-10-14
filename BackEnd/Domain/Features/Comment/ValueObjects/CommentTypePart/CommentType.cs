using Domain.Features.Comment.Exceptions.ValueObjects;
using Domain.Shared.Templates.Exceptions;

namespace Domain.Features.Comment.ValueObjects.CommentTypePart
{
    public record CommentType
    {
        //Values
        //Static
        private static Dictionary<int, CommentType> _commentTypes = new();

        //NonStatic
        public int Id { get; private set; }
        public CommentTypeEnum Name { get; private set; }
        public string Description { get; private set; }


        //Cosntructors
        public CommentType(int id)
        {
            if (!_commentTypes.TryGetValue(id, out var item))
            {
                throw new CommentTypeException
                    (
                    Messages.CommentType_Id_NotFound,
                    DomainExceptionTypeEnum.NotFound
                    );
            }
            Id = item.Id;
            Name = item.Name;
            Description = item.Description;
        }

        private CommentType(int id, CommentTypeEnum name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
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


        //=====================================================================================================
        //=====================================================================================================
        //=====================================================================================================
        //Public Methods
        public static IReadOnlyDictionary<int, CommentType> GetCommentTypes() => _commentTypes;

        //=====================================================================================================
        //=====================================================================================================
        //=====================================================================================================
        //Private Methods
    }
}
