using Domain.Features.Address.Entities;
using Domain.Features.Branch.Entities;
using Domain.Features.BranchOffer.Entities;
using Domain.Features.Comment.Entities;
using Domain.Features.Comment.ValueObjects.CommentTypePart;
using Domain.Features.Company.Entities;
using Domain.Features.Intership.Entities;
using Domain.Features.Notification.Entities;
using Domain.Features.Offer.Entities;
using Domain.Features.Person.Entities;
using Domain.Features.Recruitment.Entities;
using Domain.Features.Url.Entities;
using Domain.Features.User.Entities;

namespace Domain.Shared.Factories
{
    public interface IDomainFactory
    {
        //=================================================================================================
        //=================================================================================================
        //=================================================================================================
        //User Module

        //DomainUser Part

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
            string? login,
            DateTime? lastLoginIn,
            DateTime lastPasswordUpdate
            );

        //DomainUrl Part

        /// <summary>
        /// For Creating New DomainUrl
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="urlTypeId"></param>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        DomainUrl CreateDomainUrl
           (
           Guid userId,
           int urlTypeId,
           string path,
           string? name,
           string? description
           );

        /// <summary>
        /// For creating Regular DomainUrl, mostly for maping from Database on Domain
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="urlTypeId"></param>
        /// <param name="created"></param>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        DomainUrl CreateDomainUrl
            (
            Guid userId,
            int urlTypeId,
            DateTime created,
            string path,
            string? name,
            string? description
            );

        /// <summary>
        /// For Creating New DomainUrl
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="email"></param>
        /// <param name="previousProblemId"></param>
        /// <param name="idAppProblem"></param>
        /// <param name="userMessage"></param>
        /// <param name="notificationSenderId"></param>
        /// <param name="notificationStatusId"></param>
        /// <returns></returns>
        DomainNotification CreateDomainNotification
            (
            Guid? userId,
            string? email,
            Guid? previousProblemId,
            Guid? idAppProblem,
            string? userMessage,
            int notificationSenderId,
            int? notificationStatusId
            );

        /// <summary>
        /// For creating Regular DomainNotification, mostly for maping from Database on Domain
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <param name="email"></param>
        /// <param name="created"></param>
        /// <param name="completed"></param>
        /// <param name="previousProblemId"></param>
        /// <param name="idAppProblem"></param>
        /// <param name="userMessage"></param>
        /// <param name="response"></param>
        /// <param name="isReadedAnswerByUser"></param>
        /// <param name="notificationSenderId"></param>
        /// <param name="notificationStatusId"></param>
        /// <returns></returns>
        DomainNotification CreateDomainNotification
            (
            Guid id,
            Guid? userId,
            string? email,
            DateTime created,
            DateTime? completed,
            Guid? previousProblemId,
            Guid? idAppProblem,
            string? userMessage,
            string? response,
            string isReadedAnswerByUser,
            int notificationSenderId,
            int notificationStatusId
            );

        //=================================================================================================
        //=================================================================================================
        //=================================================================================================
        //Person Module

        /// <summary>
        /// For Creating New DomainPerson
        /// </summary>
        /// <param name="id"></param>
        /// <param name="segmentUrl"></param>
        /// <param name="contactEmail"></param>
        /// <param name="name"></param>
        /// <param name="surname"></param>
        /// <param name="birthDate"></param>
        /// <param name="contactPhoneNum"></param>
        /// <param name="description"></param>
        /// <param name="isStudent"></param>
        /// <param name="isPublicProfile"></param>
        /// <param name="addressId"></param>
        /// <returns></returns>
        DomainPerson CreateDomainPerson
            (
            Guid id,
            string? segmentUrl,
            string contactEmail,
            string name,
            string surname,
            DateOnly? birthDate,
            string? contactPhoneNum,
            string? description,
            bool isStudent,
            bool isPublicProfile,
            Guid? addressId
            );

        /// <summary>
        /// For creating Regular DomainPerson, mostly for maping from Database on Domain
        /// </summary>
        /// <param name="id"></param>
        /// <param name="segmentUrl"></param>
        /// <param name="created"></param>
        /// <param name="contactEmail"></param>
        /// <param name="name"></param>
        /// <param name="surname"></param>
        /// <param name="birthDate"></param>
        /// <param name="contactPhoneNum"></param>
        /// <param name="description"></param>
        /// <param name="isStudent"></param>
        /// <param name="isPublicProfile"></param>
        /// <param name="addressId"></param>
        /// <returns></returns>
        DomainPerson CreateDomainPerson
            (
            Guid id,
            string? segmentUrl,
            DateOnly created,
            string? contactEmail,
            string? name,
            string? surname,
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
        //Company Module

        //Company Part

        /// <summary>
        /// For Creating New DomainCompany
        /// </summary>
        /// <param name="id"></param>
        /// <param name="segmentUrl"></param>
        /// <param name="contactEmail"></param>
        /// <param name="name"></param>
        /// <param name="regon"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        DomainCompany CreateDomainCompany
            (
            Guid id,
            string? segmentUrl,
            string contactEmail,
            string name,
            string regon,
            string? description
            );

        /// <summary>
        /// For creating Regular DomainCompany, mostly for maping from Database on Domain
        /// </summary>
        /// <param name="id"></param>
        /// <param name="segmentUrl"></param>
        /// <param name="contactEmail"></param>
        /// <param name="name"></param>
        /// <param name="regon"></param>
        /// <param name="description"></param>
        /// <param name="createDate"></param>
        /// <returns></returns>
        DomainCompany CreateDomainCompany
            (
            Guid id,
            string? segmentUrl,
            string? contactEmail,
            string? name,
            string? regon,
            string? description,
            DateOnly created
            );

        /// <summary>
        /// For Creating New DomainBranch
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="addressId"></param>
        /// <param name="segmentUrl"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        DomainBranch CreateDomainBranch
            (
            Guid companyId,
            Guid addressId,
            string? segmentUrl,
            string name,
            string? description
            );

        /// <summary>
        /// For creating Regular DomainBranch, mostly for maping from Database on Domain
        /// </summary>
        /// <param name="id"></param>
        /// <param name="companyId"></param>
        /// <param name="addressId"></param>
        /// <param name="segmentUrl"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        DomainBranch CreateDomainBranch
            (
            Guid id,
            Guid companyId,
            Guid? addressId,
            string? segmentUrl,
            string? name,
            string? description
            );



        //DomainOffer part

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
           Guid id,
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
           Guid id,
           string name,
           string description,
           decimal? minSalary,
           decimal? maxSalary,
           string? isNegotiatedSalary,
           string isForStudents
           );


        //BranchOffer

        /// <summary>
        /// For Creating New DomainBranchOffer
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="offerId"></param>
        /// <param name="publishStart"></param>
        /// <param name="publishEnd"></param>
        /// <param name="workStart"></param>
        /// <param name="workEnd"></param>
        /// <returns></returns>
        DomainBranchOffer CreateDomainBranchOffer
           (
            Guid branchId,
            Guid offerId,
            DateTime publishStart,
            DateTime? publishEnd,
            DateOnly? workStart,
            DateOnly? workEnd
           );

        /// <summary>
        /// For creating Regular DomainBranchOffer, mostly for maping from Database on Domain
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="offerId"></param>
        /// <param name="created"></param>
        /// <param name="publishStart"></param>
        /// <param name="publishEnd"></param>
        /// <param name="workStart"></param>
        /// <param name="workEnd"></param>
        /// <param name="lastUpdate"></param>
        /// <returns></returns>
        DomainBranchOffer CreateDomainBranchOffer
           (
            Guid id,
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

        /// <summary>
        /// For Creating New DomainRecruitment
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="branchId"></param>
        /// <param name="offerId"></param>
        /// <param name="created"></param>
        /// <param name="personMessage"></param>
        /// <returns></returns>
        DomainRecruitment CreateDomainRecruitment
            (
            Guid personId,
            Guid branchOfferId,
            string? personMessage
            );

        /// <summary>
        /// For creating Regular DomainRecruitment, mostly for maping from Database on Domain
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="branchId"></param>
        /// <param name="offerId"></param>
        /// <param name="created"></param>
        /// <param name="applicationDate"></param>
        /// <param name="personMessage"></param>
        /// <param name="companyResponse"></param>
        /// <param name="acceptedRejected"></param>
        /// <returns></returns>
        DomainRecruitment CreateDomainRecruitment
            (
            Guid? id,
            Guid personId,
            Guid branchOfferId,
            DateTime created,
            string? personMessage,
            string? companyResponse,
            string? isAccepted
            );

        /// <summary>
        /// For Creating New DomainIntership
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="branchId"></param>
        /// <param name="offerId"></param>
        /// <param name="created"></param>
        /// <param name="contractNumber"></param>
        /// <returns></returns>
        DomainIntership CreateDomainInternship
            (
            Guid id,
            DateOnly contractStartDate,
            DateOnly? contractEndDate,
            string contractNumber
            );

        /// <summary>
        /// For creating Regular DomainIntership, mostly for maping from Database on Domain
        /// </summary>
        /// <param name="id"></param>
        /// <param name="personId"></param>
        /// <param name="branchId"></param>
        /// <param name="offerId"></param>
        /// <param name="created"></param>
        /// <param name="contractNumber"></param>
        /// <returns></returns>
        DomainIntership CreateDomainIntership
            (
            Guid id,
            DateTime created,
            DateOnly contractStartDate,
            DateOnly? contractEndDate,
            string contractNumber
            );

        /// <summary>
        /// For Creating New DomainComment
        /// </summary>
        /// <param name="internshipId"></param>
        /// <param name="commentTypeId"></param>
        /// <param name="description"></param>
        /// <param name="evaluation"></param>
        /// <returns></returns>
        DomainComment CreateDomainComment
            (
            Guid internshipId,
            CommentSenderEnum sender,
            CommentTypeEnum type,
            string description,
            int? evaluation
            );

        /// <summary>
        /// For creating Regular DomainComment, mostly for maping from Database on Domain
        /// </summary>
        /// <param name="internshipId"></param>
        /// <param name="commentTypeId"></param>
        /// <param name="created"></param>
        /// <param name="description"></param>
        /// <param name="evaluation"></param>
        /// <returns></returns>
        DomainComment CreateDomainComment
             (
            Guid internshipId,
            int commentTypeId,
            DateTime created,
            string description,
            int? evaluation
            );


        //=================================================================================================
        //=================================================================================================
        //=================================================================================================
        //Address Module
        /*
                /// <summary>
                /// For Creating New DomainAddress
                /// </summary>
                /// <param name="divisionId"></param>
                /// <param name="streetId"></param>
                /// <param name="buildingNumber"></param>
                /// <param name="apartmentNumber"></param>
                /// <param name="zipCode"></param>
                /// <returns></returns>
                DomainAddress CreateDomainAddress
                    (
                    int divisionId,
                    int streetId,
                    string buildingNumber,
                    string? apartmentNumber,
                    string zipCode
                    );*/

        /// <summary>       
        /// For creating Regular DomainAddress, mostly for maping from Database on Domain
        /// </summary>
        /// <param name="id"></param>
        /// <param name="divisionId"></param>
        /// <param name="streetId"></param>
        /// <param name="buildingNumber"></param>
        /// <param name="apartmentNumber"></param>
        /// <param name="zipCode"></param>
        /// <returns></returns>
        DomainAddress CreateDomainAddress(
            Guid? id,
            int divisionId,
            int? streetId,
            string buildingNumber,
            string? apartmentNumber,
            string zipCode,
            double lon,
            double lat);
        //=================================================================================================
        //=================================================================================================
        //=================================================================================================
    }
}
