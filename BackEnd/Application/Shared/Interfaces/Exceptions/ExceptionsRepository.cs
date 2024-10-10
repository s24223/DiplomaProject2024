using Application.Shared.Exceptions.AppExceptions;
using Domain.Features.Address.Exceptions.Entities;
using Domain.Features.Branch.Exceptions.Entities;
using Domain.Features.Company.Exceptions.Entities;
using Domain.Features.Url.Exceptions.Entities;
using Domain.Shared.Exceptions.UserExceptions.ValueObjectsExceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Application.Shared.Interfaces.Exceptions
{
    public class ExceptionsRepository : IExceptionsRepository
    {
        public Exception ConvertEFDbException
            (
            Exception ex
            )
        {
            if (ex is DbUpdateException && ex.InnerException is SqlException sqlEx)
            {
                var number = sqlEx.Number;
                var message = sqlEx.Message;
                /*
                 * User Module
                CHECK_UserProblem_Status
                

                CHECK_Person_IsStudent
                CHECK_Person_IsPublicProfile
                UNIQUE_Person_UrlSegment
                UNIQUE_Person_ContactEmail
                

                CHECK_Offer_MinSalary
                CHECK_Offer_MaxSalary
                CHECK_Offer_IsNegotiatedSalary
                CHECK_Offer_IsForStudents

                CHECK_BranchOffer_PublishEnd
                CHECK_BranchOffer_WorkStart
                CHECK_BranchOffer_WorkEnd



                CHECK_Comment_Evaluation
                 */
                switch (number)
                {
                    case 2627: // Naruszenie klucza unikalnego
                        //User Module
                        if (message.Contains("UNIQUE_User_Login"))
                        {
                            return new EmailException(Messages.NotUniqueUserEmailLogin);
                        }
                        else if (message.Contains("UNIQUE_Url_Path"))
                        {
                            return new UrlException(Messages.NotUniqueUrlPath);
                        }

                        //Company Module
                        else if (message.Contains("Company_pk"))
                        {
                            return new CompanyException(Messages.IsExistCompany);
                        }
                        else if (message.Contains("UNIQUE_Company_UrlSegment"))
                        {
                            return new CompanyException(Messages.NotUniqueCompanyUrlSegment);
                        }
                        else if (message.Contains("UNIQUE_Company_ContactEmail"))
                        {
                            return new CompanyException(Messages.NotUniqueCompanyContactEmail);
                        }
                        else if (message.Contains("UNIQUE_Company_Name"))
                        {
                            return new CompanyException(Messages.NotUniqueCompanyName);
                        }
                        else if (message.Contains("UNIQUE_Company_Regon"))
                        {
                            return new CompanyException(Messages.NotUniqueCompanyRegon);
                        }
                        else if (message.Contains("UNIQUE_Branch_UrlSegment"))
                        {
                            return new BranchException(Messages.NotUniqueBranchUrlSegment);
                        }
                        break;
                    case 547: // Naruszenie klucza obcego lub CHECK
                        Console.WriteLine(ex.Message);
                        if (message.Contains("CHECK_UserProblem_Status"))
                        {
                            //throw new UserProblemStatusException(Messages.NotExistUserProblemStatus);
                            //"Copy_of_Address_Street"
                            //"Address_Division"
                            //Branch_Address
                        }
                        //Company Module
                        else if (message.Contains("Branch_Address"))
                        {
                            return new AddressException(Messages.NotFoundAddress);
                        }
                        break;
                    default:
                        return ex;
                }
            }
            return ex;
        }



        public Exception ConvertSqlClientDbException
            (
            Exception ex,
            string? inputData = null
            )
        {
            switch (ex)
            {
                /*case SqlException:
                    return new SqlClientImplementationException
                    (
                    _provider.ExceptionsMessageProvider().GenerateExceptionMessage
                        (
                        GetType(),
                        method,
                        ex,
                        inputData
                        )
                    );*/
                default:
                    return new SqlClientImplementationException
                    (
                    inputData
                    );
            }
        }

    }
}
