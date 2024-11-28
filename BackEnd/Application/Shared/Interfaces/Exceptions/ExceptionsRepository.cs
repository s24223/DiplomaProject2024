using Application.Shared.Exceptions.AppExceptions;
using Domain.Features.Address.Exceptions.Entities;
using Domain.Features.Branch.Exceptions.Entities;
using Domain.Features.Company.Exceptions.Entities;
using Domain.Features.Offer.Exceptions.Entities;
using Domain.Features.Person.Exceptions.Entities;
using Domain.Features.Url.Exceptions.Entities;
using Domain.Shared.Exceptions.ValueObjects;
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
                Console.WriteLine(number + " " + message);
                var exceptions2627 = new Dictionary<string, Func<Exception>>
                {
                    //====================================================================================
                    //User Module
                    
                    //User Part
                    //Context: Każdy urzytkownik w rmach systemu powinien miec unikalny login
                    {"User_UNIQUE_Login", () => new EmailException
                        (
                            Messages2.DatabaseConstraint_UNIQUE_User_Login
                        )
                    },

                    //Url Part
                    //Context: Podany Url.Path przez Urzykownika nie może powtarzac sie
                    {"UNIQUE_Url_Path", () =>  new UrlException
                        (
                            Messages2.DatabaseConstraint_UNIQUE_Url_Path
                        )
                    },


                    //====================================================================================
                    //Person Module
                    { "Person_pk", () => new  PersonException("Person Profiile Exist") },
                    /*
                    UNIQUE_Person_UrlSegment
                    UNIQUE_Person_ContactEmail
                    UNIQUE_Person_ContactPhoneNum
                     */


                    //====================================================================================
                    //Company Module

                    //Company Part
                    //Context: Użytkownik nie może uworzyć swoj profil Company wielokrotnie
                    {"Company_pk", () => new CompanyException
                        (
                            Messages2.DatabaseConstraint_Company_pk_Exist
                        )
                    },
                    //Context: Każda firma posiada unikalny Urlsegment dla łatwiejszego wyszukania
                    {"UNIQUE_Company_UrlSegment", () => new CompanyException
                        (
                            Messages2.DatabaseConstraint_UNIQUE_Company_UrlSegment
                        )
                    },
                    //Context: Każda firma posiada unikalny ContactEmail w wypadku kontaktu kandydata do firmy była jednoznaczność
                    {"UNIQUE_Company_ContactEmail", () => new CompanyException
                        (
                            Messages2.DatabaseConstraint_UNIQUE_Company_ContactEmail
                        )
                    },
                    //Context: Wszystkie firmy zarejestrowane w Polsce powinny miec unikalną nazwę 
                    {"UNIQUE_Company_Name", () => new CompanyException
                        (
                            Messages2.DatabaseConstraint_UNIQUE_Company_Name
                        )
                    },
                    //Context: Wszystkie firmy zarejestrowane w Polsce powinny miec unikalny REGON
                    {"UNIQUE_Company_Regon", () => new CompanyException
                        (
                            Messages2.DatabaseConstraint_UNIQUE_Company_Regon
                        )
                    },                

                    //Branch part
                    //Context: W kontekcie firmy UrlSegment oddziału musi byc unikalny np.: randCompany/gdansk, randCompany/warszawa
                    { "UNIQUE_Branch_UrlSegment", () => new BranchException
                        (
                            Messages2.DatabaseConstraint_UNIQUE_Branch_UrlSegment
                        )
                    },


                    //====================================================================================
                    //Intership Module

                };

                var exceptions547 = new Dictionary<string, Func<Exception>>
                {
                    //====================================================================================
                    //User Module
                    
                    /*
                    CHECK_UserProblem_Status
                    */


                    //====================================================================================
                    //Person Module
                    /*
                     * 
                    CHECK_Person_IsStudent
                    CHECK_Person_IsPublicProfile
                    */

                    //====================================================================================
                    //Company Module

                    //Branch part
                    //Context: Przy tworzeniu objektu Branch (Oddział) został podany nieisniejący AddressId, trzeba najpierw utworzyć Address który zróci id i wpisac go w pole
                    {"Branch_Address", () => new AddressException
                        (
                        Messages2.DatabaseConstraint_Branch_Address_NotFound,
                        DomainExceptionTypeEnum.NotFound
                        )
                    },
                    { "Person_Address", () => new  PersonException("Address not Exist", DomainExceptionTypeEnum.NotFound) },


                    {"CHECK_BranchOffer_PublishEnd", () =>  new CompanyException(
                        Messages2.DatabaseConstraint_CHECK_BranchOffer_PublishEnd
                        )
                    },
                    {"CHECK_BranchOffer_WorkStart", () =>  new CompanyException(
                        Messages2.DatabaseConstraint_CHECK_BranchOffer_WorkStart
                        )
                    },
                    {"CHECK_BranchOffer_WorkEnd", () =>  new CompanyException(
                        Messages2.DatabaseConstraint_CHECK_BranchOffer_WorkEnd
                        )
                    },

                    {"CHECK_Offer_MinSalary", () =>  new OfferException(
                        Messages2.DatabaseConstraint_CHECK_Offer_MinSalary
                        )
                    },
                    {"CHECK_Offer_MaxSalary", () =>  new OfferException(
                        Messages2.DatabaseConstraint_CHECK_Offer_MaxSalary
                        )
                    },
                    {"CHECK_Offer_IsNegotiatedSalary", () =>  new OfferException(
                        Messages2.DatabaseConstraint_CHECK_Offer_IsNegotiatedSalary
                        )
                    },
                    {"CHECK_Offer_IsForStudents", () =>  new OfferException(
                        Messages2.DatabaseConstraint_CHECK_Offer_IsForStudents
                        )
                    },
                    /*
                    CHECK_BranchOffer_PublishEnd
                    CHECK_BranchOffer_WorkStart
                    CHECK_BranchOffer_WorkEnd

                    CHECK_UserProblem_Status
                    Url_UrlType
                    Url_User
                    UserProblem_User
                     */


                    //====================================================================================
                    //Intership Module
                };

                switch (number)
                {
                    // Naruszenie klucza unikalnego
                    case 2627:
                        foreach (var key in exceptions2627.Keys)
                        {
                            if (message.Contains(key))
                            {
                                return exceptions2627[key]();
                            }
                        }
                        break;
                    // Naruszenie klucza obcego lub CHECK
                    case 547:
                        foreach (var key in exceptions547.Keys)
                        {
                            if (message.Contains(key))
                            {
                                return exceptions547[key]();
                            }
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
