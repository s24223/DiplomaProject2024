using Domain.Features.Comment.Reposoitories;
using Domain.Features.Comment.ValueObjects.CommentTypePart;

namespace DomainTests.Fakes
{
    public class CommentTypeFake : ICommentTypeRepo
    {
        public Dictionary<int, DomainCommentType> GetDictionary()
        {
            return new Dictionary<int, DomainCommentType>()
            {
                {
                    (int)(CommentTypeEnum.Dzienny),
                    new DomainCommentType((int)(CommentTypeEnum.Dzienny), "Day: Company", "Desc")
                },
                {
                    (int)(CommentTypeEnum.Dzienny)+1,
                    new DomainCommentType((int)(CommentTypeEnum.Dzienny +1), "Day :Person", "Desc")
                },
                {
                    (int)(CommentTypeEnum.Tygodniowy + 1),
                    new DomainCommentType((int)(CommentTypeEnum.Tygodniowy + 1), "Week:Person", "Desc")
                },{
                    (int)(CommentTypeEnum.Tygodniowy),
                    new DomainCommentType((int)(CommentTypeEnum.Tygodniowy), "Week: Company", "Desc")
                },
                {
                    (int)(CommentTypeEnum.Miesięczny),
                    new DomainCommentType((int)(CommentTypeEnum.Miesięczny), "Month: Company", "Desc")
                },{
                    (int)(CommentTypeEnum.Miesięczny + 1),
                    new DomainCommentType((int)(CommentTypeEnum.Miesięczny + 1), "Month:Person", "Desc")
                },
            };
        }

        public DomainCommentType GetValue(int id)
        {
            return GetDictionary()[id];
        }
    }
}
