using Domain.Features.Address.Entities;
using Domain.Features.Branch.Entities;
using Domain.Features.BranchOffer.Entities;
using Domain.Features.Comment.Entities;
using Domain.Features.Company.Entities;
using Domain.Features.Intership.Entities;
using Domain.Features.Offer.Entities;
using Domain.Features.Person.Entities;
using Domain.Features.Recruitment.Entities;
using Domain.Features.Url.Entities;
using Domain.Features.User.Entities;
using Domain.Features.UserProblem.Entities;

namespace Domain.Shared.Factories
{
    public interface IDomainFactory
    {
        //=================================================================================================
        //=================================================================================================
        //=================================================================================================
        //User Part
        /// <summary>
        /// For Creating New DomainUser
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        DomainUser CreateDomainUser(string login);

        /// <summary>
        /// For creating Regular DomainUser, mostly for maping from Database on Domain
        /// </summary>
        /// <param name="id"></param>
        /// <param name="login"></param>
        /// <param name="lastLoginIn"></param>
        /// <param name="lastPasswordUpdate"></param>
        /// <returns></returns>
        DomainUser CreateDomainUser
            (
            Guid id,
            string login,
            DateTime? lastLoginIn,
            DateTime lastPasswordUpdate
            );


        /// <summary>
        /// For Creating New DomainUserProblem
        /// </summary>
        /// <param name="userMessage"></param>
        /// <param name="previousProblemId"></param>
        /// <param name="email"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        DomainUserProblem CreateDomainUserProblem
            (
            string userMessage,
            Guid? previousProblemId,
            string? email,
            Guid? userId
            );

        /// <summary>
        /// For creating Regular DomainUserProblem, mostly for maping from Database on Domain
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dateTime"></param>
        /// <param name="userMessage"></param>
        /// <param name="response"></param>
        /// <param name="previousProblemId"></param>
        /// <param name="email"></param>
        /// <param name="status"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        DomainUserProblem CreateDomainUserProblem
            (
            Guid id,
            DateTime created,
            string userMessage,
            string? response,
            Guid? previousProblemId,
            string? email,
            string status,
            Guid? userId
            );


        DomainUrl CreateDomainUrl
            (
            Guid userId,
            int urlTypeId,
            DateTime created,
            string path,
            string? name,
            string? description
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
        //Company Part
        /// <summary>
        /// For Creating New DomainCompany
        /// </summary>
        /// <param name="id"></param>
        /// <param name="urlSegment"></param>
        /// <param name="contactEmail"></param>
        /// <param name="name"></param>
        /// <param name="regon"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        DomainCompany CreateDomainCompany
            (
            Guid id,
            string? urlSegment,
            string contactEmail,
            string name,
            string regon,
            string? description
            );
        /// <summary>
        /// For creating Regular DomainCompany, mostly for maping from Database on Domain
        /// </summary>
        /// <param name="id"></param>
        /// <param name="urlSegment"></param>
        /// <param name="contactEmail"></param>
        /// <param name="name"></param>
        /// <param name="regon"></param>
        /// <param name="description"></param>
        /// <param name="createDate"></param>
        /// <returns></returns>
        DomainCompany CreateDomainCompany
            (
            Guid id,
            string? urlSegment,
            string contactEmail,
            string name,
            string regon,
            string? description,
            DateOnly? created
            );

        /// <summary>
        /// For Creating New DomainBranch
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="addressId"></param>
        /// <param name="urlSegment"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        DomainBranch CreateDomainBranch
            (
            Guid companyId,
            Guid addressId,
            string? urlSegment,
            string name,
            string? description
            );

        /// <summary>
        /// For creating Regular DomainBranch, mostly for maping from Database on Domain
        /// </summary>
        /// <param name="id"></param>
        /// <param name="companyId"></param>
        /// <param name="addressId"></param>
        /// <param name="urlSegment"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        DomainBranch CreateDomainBranch
            (
            Guid? id,
            Guid companyId,
            Guid addressId,
            string? urlSegment,
            string name,
            string? description
            );

        /// <summary>
        /// For Creating New DomainOffer
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="minSalary"></param>
        /// <param name="maxSalary"></param>
        /// <param name="NegotiatedSalary"></param>
        /// <param name="forStudents"></param>
        /// <returns></returns>
        DomainOffer CreateDomainOffer
           (
           string name,
           string description,
           decimal? minSalary,
           decimal? maxSalary,
           bool? isNegotiatedSalary,
           bool isForStudents
           );

        /// <summary>
        /// For Creating New DomainOffer
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="minSalary"></param>
        /// <param name="maxSalary"></param>
        /// <param name="NegotiatedSalary"></param>
        /// <param name="forStudents"></param>
        /// <returns></returns>
        DomainOffer CreateDomainOffer
          (
           string name,
           string description,
           decimal? minSalary,
           decimal? maxSalary,
           string? isNegotiatedSalary,
           string isForStudents
          );

        /// <summary>
        /// For creating Regular DomainOffer, mostly for maping from Database on Domain
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="minSalary"></param>
        /// <param name="maxSalary"></param>
        /// <param name="NegotiatedSalary"></param>
        /// <param name="forStudents"></param>
        /// <returns></returns>
        DomainOffer CreateDomainOffer
           (
           Guid? id,
           string name,
           string description,
           decimal? minSalary,
           decimal? maxSalary,
           bool? isNegotiatedSalary,
           bool isForStudents
           );

        /// <summary>
        /// For creating Regular DomainOffer, mostly for maping from Database on Domain
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="minSalary"></param>
        /// <param name="maxSalary"></param>
        /// <param name="NegotiatedSalary"></param>
        /// <param name="forStudents"></param>
        /// <returns></returns>
        DomainOffer CreateDomainOffer
           (
           Guid? id,
           string name,
           string description,
           decimal? minSalary,
           decimal? maxSalary,
           string? isNegotiatedSalary,
           string isForStudents
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

        DomainIntership CreateDomainInternship(
            string contactNumber,
            Guid personId,
            Guid branchId,
            Guid offerId,
            DateTime created
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
        //Address Part
        DomainAddress CreateDomainAddress
            (
            Guid? id,
            int divisionId,
            int streetId,
            string buildingNumber,
            string? apartmentNumber,
            string zipCode
            );

        DomainAddress CreateDomainAddress
            (
            int divisionId,
            int streetId,
            string buildingNumber,
            string? apartmentNumber,
            string zipCode
            );
    }
}
