using Domain.Shared.Templates.Exceptions;

namespace Domain.Features.Recruitment.Exceptions.Entities
{
    public class RecruitmentException : DomainException
    {
        public RecruitmentException
            (
            string? message,
            DomainExceptionTypeEnum type = DomainExceptionTypeEnum.BadInputData
            ) : base(message, type)
        {
        }
    }
}
