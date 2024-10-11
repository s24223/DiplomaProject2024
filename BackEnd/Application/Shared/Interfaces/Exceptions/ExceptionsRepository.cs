using Application.Shared.Exceptions.AppExceptions;
using Domain.Features.Address.Exceptions.Entities;
using Domain.Features.Branch.Exceptions.Entities;
using Domain.Features.Company.Exceptions.Entities;
using Domain.Features.Url.Exceptions.Entities;
using Domain.Shared.Exceptions.UserExceptions.ValueObjectsExceptions;
using Domain.Shared.Templates.Exceptions;
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
                Person_pk
                CHECK_Person_IsStudent
                CHECK_Person_IsPublicProfile
                UNIQUE_Person_UrlSegment
                UNIQUE_Person_ContactEmail
                Person_User
                Person_Address


                Company_User
                CHECK_Offer_MinSalary
                CHECK_Offer_MaxSalary
                CHECK_Offer_IsNegotiatedSalary
                CHECK_Offer_IsForStudents             

                CHECK_Comment_Evaluation
                 */
                switch (number)
                {

                    //====================================================================================
                    //====================================================================================
                    //====================================================================================
                    // Naruszenie klucza unikalnego
                    case 2627:
                        //====================================================================================
                        //User Module
                        if (message.Contains("UNIQUE_User_Login"))
                        {
                            //Context: Każdy urzytkownik w rmach systemu powinien miec unikalny login
                            return new EmailException(Messages.NotUniqueUserEmailLogin);
                        }
                        else if (message.Contains("UNIQUE_Url_Path"))
                        {
                            //Context: Podany Url.Path przez Urzykownika nie może powtarzac sie
                            return new UrlException(Messages.NotUniqueUrlPath);
                        }


                        //====================================================================================
                        //Company Module

                        //Company Part
                        else if (message.Contains("Company_pk"))
                        {
                            //Context: Użytkownik nie może uworzyć swoj profil Company wielokrotnie
                            return new CompanyException(Messages.IsExistCompany);
                        }
                        else if (message.Contains("UNIQUE_Company_UrlSegment"))
                        {
                            //Context: Każda firma posiada unikalny Urlsegment dla łatwiejszego wyszukania
                            return new CompanyException(Messages.NotUniqueCompanyUrlSegment);
                        }
                        else if (message.Contains("UNIQUE_Company_ContactEmail"))
                        {
                            //Context: Każda firma posiada unikalny ContactEmail w wypadku kontaktu
                            // kandydata do firmy była jednoznaczność
                            return new CompanyException(Messages.NotUniqueCompanyContactEmail);
                        }
                        else if (message.Contains("UNIQUE_Company_Name"))
                        {
                            //Context: Wszystkie firmy zarejestrowane w Polsce powinny miec unikalną nazwę 
                            return new CompanyException(Messages.NotUniqueCompanyName);
                        }
                        else if (message.Contains("UNIQUE_Company_Regon"))
                        {
                            //Context: Wszystkie firmy zarejestrowane w Polsce powinny miec unikalny REGON
                            return new CompanyException(Messages.NotUniqueCompanyRegon);
                        }

                        //Branch part
                        else if (message.Contains("UNIQUE_Branch_UrlSegment"))
                        {
                            //Context: W kontekcie firmy UrlSegment oddziału musi byc unikalny
                            //np.: randCompany/gdansk, randCompany/warszawa 
                            return new BranchException(Messages.NotUniqueBranchUrlSegment);
                        }
                        //====================================================================================
                        break;

                    //====================================================================================
                    //====================================================================================
                    //====================================================================================
                    // Naruszenie klucza obcego lub CHECK
                    case 547:
                        //====================================================================================
                        Console.WriteLine(ex.Message);
                        if (message.Contains("CHECK_UserProblem_Status"))
                        {
                            //Context: Problem po stronie Backendu 
                        }
                        /*
                        CHECK_UserProblem_Status
                        Url_UrlType
                        Url_User
                        UserProblem_User
                         */

                        //====================================================================================
                        //Company Module

                        //Company Part
                        else if (message.Contains("Branch_Address"))
                        {
                            //Context: Przy tworzeniu objektu Branch (Oddział) został podany nieisniejący
                            //AddressId, trzeba najpierw utworzyć Address który zróci id i wpisac go w pole
                            return new AddressException
                                (
                                Messages.NotFoundAddress,
                                DomainExceptionTypeEnum.NotFound
                                );
                        }

                        //Branch part
                        else if (message.Contains("Branch_Company"))
                        {
                            //Context:  Przy tworzeniu objektu Branch (Oddział) nie został utworzony profil
                            //Company(Firmy), nalezy najpierw utworzyc firmę a wtedy mozna utworzyc Branch
                            return new CompanyException
                                (
                                Messages.NotFoundCompany,
                                DomainExceptionTypeEnum.NotFound
                                );
                        }
                        else if (message.Contains("BranchOffer_Branch"))
                        {
                            //Context: Przy tworzeniu objektu BranchOffer 
                            return new CompanyException
                                (
                                Messages.NotFoundBranch,
                                DomainExceptionTypeEnum.NotFound
                                );
                        }
                        else if (message.Contains("BranchOffer_Offer"))
                        {
                            //Context: Przy tworzeniu objektu BranchOffer nie został znalieziony Offer
                            //(Oderta) o podanym Id
                            return new CompanyException(
                                Messages.NotFoundOffer,
                                DomainExceptionTypeEnum.NotFound
                                );
                        }
                        /*
                         
                        CHECK_BranchOffer_PublishEnd
                        CHECK_BranchOffer_WorkStart
                        CHECK_BranchOffer_WorkEnd
                         */
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
