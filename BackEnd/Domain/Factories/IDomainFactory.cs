using Domain.Entities.CompanyPart;
using Domain.Entities.PersonPart;
using Domain.Entities.RecrutmentPart;
using Domain.Entities.UserPart;

namespace Domain.Factories
{
    public interface IDomainFactory
    {
        //=================================================================================================
        //=================================================================================================
        //=================================================================================================
        //Company Part
        DomainBranch CreateDomainBranch
            (
            Guid? id,
            Guid companyId,
            Guid addressId,
            string? urlSegment,
            string name,
            string? description
            );

        DomainBranchOffer CreateDomainBranchOffer
           (
           Guid branchId,
           Guid offerId,
           DateTime created,
           DateTime publishStart,
           DateTime? publishEnd,
           DateOnly? workStart,
           DateOnly? workEnd,
           DateTime lastUpdate
           );

        DomainCompany CreateDomainCompany
            (
            Guid id,
            string? urlSegment,
            string contactEmail,
            string name,
            string regon,
            string? description,
            DateOnly? createDate
            );

        DomainOffer CreateDomainOffer
           (
           Guid? id,
           string name,
           string description,
           decimal? minSalary,
           decimal? maxSalary,
           string? NegotiatedSalary,
           string forStudents
           );


        //=================================================================================================
        //=================================================================================================
        //=================================================================================================
        //Person Part
        DomainPerson CreateDomainPerson
            (
            Guid id,
            string? urlSegment,
            DateOnly? createDate,
            string contactEmail,
            string name,
            string surname,
            DateOnly? birthDate,
            string? contactPhoneNum,
            string? description,
            string isStudent,
            string isPublicProfile,
            Guid? addressId
            );
        //=================================================================================================
        //=================================================================================================
        //=================================================================================================
        //Recruitment Part
        DomainComment CreateDomainComment
            (
            Guid internshipId,
            int commentTypeId,
            DateTime published,
            string description,
            int? evaluation
            );

        DomainIntership CreateDomainIntership
            (
            Guid? id,
            Guid personId,
            Guid branchId,
            Guid offerId,
            DateTime created,
            string contractNumber
            );

        DomainRecruitment CreateDomainRecruitment
            (
            Guid personId,
            Guid branchId,
            Guid offerId,
            DateTime created,
            DateTime? applicationDate,
            string? personMessage,
            string? companyResponse,
            string? acceptedRejected
            );

        //=================================================================================================
        //=================================================================================================
        //=================================================================================================
        //User Part
        DomainUrl CreateDomainUrl
            (
            Guid userId,
            int urlTypeId,
            DateTime publishDate,
            string url,
            string? name,
            string? description
            );

        DomainUser CreateDomainUser
            (
            Guid? id,
            string loginEmail,
            DateTime? lastLoginIn,
            DateTime? lastUpdatePassword
            );

        DomainUserProblem CreateDomainUserProblem
            (
            Guid? id,
            DateTime? dateTime,
            string userMessage,
            string? response,
            Guid? previousProblemId,
            string? email,
            string? status,
            Guid? userId
            );

        DomainIntership CreateDomainInternship(
            string contactNumber,
            Guid personId,
            Guid branchId,
            Guid offerId,
            DateTime created
            );
    }
}
