using Application.Shared.Exceptions.AppExceptions;
using Domain.Exceptions.UserExceptions.EntitiesExceptions;
using Domain.Exceptions.UserExceptions.ValueObjectsExceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Application.Shared.Interfaces.Exceptions
{
    public class ExceptionsRepository : IExceptionsRepository
    {
        public Exception ConvertEFDbException
            (
            Exception ex,
            Type classType,
            MethodBase? method
            )
        {
            if (ex is DbUpdateException && ex.InnerException is SqlException sqlEx)
            {
                var number = sqlEx.Number;
                var message = sqlEx.Message;
                switch (number)
                {
                    case 2627: // Naruszenie klucza unikalnego
                        if (message.Contains("UNIQUE_User_Login"))
                        {
                            return new EmailException(Messages.NotUniqueUserEmailLogin);
                        }
                        else if (message.Contains("UNIQUE_Url_Path"))
                        {
                            return new UrlException(Messages.NotUniqueUrlPath);
                        }
                        break;
                    case 547: // Naruszenie klucza obcego lub CHECK
                        if (message.Contains("CHECK_UserProblem_Status"))
                        {
                            //throw new UserProblemStatusException(Messages.NotExistUserProblemStatus);

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
