using Domain.Features.Comment.Exceptions.ValueObjects;
using Domain.Features.Comment.ValueObjects.CommentTypePart;
using Domain.Features.Recruitment.ValueObjects.Identificators;

namespace Domain.Features.Comment.ValueObjects.Identificators
{
    public record CommentId
    {
        //Values
        public RecrutmentId IntershipId { get; private set; }
        public int CommentTypeId { get; private set; }
        public DateTime Created { get; private set; }

        //Cosntructor
        public CommentId
            (
            RecrutmentId intershipId,
            CommentSenderEnum sender,
            CommentTypeEnum type,
            DateTime created
            )
        {
            if (!Enum.IsDefined<CommentTypeEnum>(type))
            {
                throw new CommentTypeException(
                    $"{Messages.CommentType_Enum_IdNotFound}: {(int)type}",
                    Shared.Templates.Exceptions.DomainExceptionTypeEnum.NotFound);
            }

            IntershipId = intershipId;
            Created = created;

            if ((int)sender == 2)
            {
                CommentTypeId = (int)type + 1;
            }
            else
            {
                CommentTypeId = (int)type;
            }
        }

        public CommentId
            (
            RecrutmentId intershipId,
            int commentTypeId,
            DateTime created
            )
        {
            IntershipId = intershipId;
            Created = created;
            CommentTypeId = commentTypeId;
        }
    }
}
