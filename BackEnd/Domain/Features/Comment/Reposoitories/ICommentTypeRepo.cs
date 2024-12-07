using Domain.Features.Comment.ValueObjects.CommentTypePart;

namespace Domain.Features.Comment.Reposoitories
{
    public interface ICommentTypeRepo
    {
        Dictionary<int, DomainCommentType> GetDictionary();
        DomainCommentType GetValue(int id);
    }
}
