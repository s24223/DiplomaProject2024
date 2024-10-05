using Application.Database;
using Application.Shared.DTOs.Response;
using Application.Shared.Exceptions.UserExceptions;
using Domain.Exceptions.UserExceptions.EntitiesExceptions;
using Domain.Exceptions.UserExceptions.ValueObjectsExceptions;
using Infrastructure.Exceptions.AppExceptions;
using System.Diagnostics;
using System.Text.Json;

namespace BackEnd.Middlewares.CustomMiddlewares
{
    public class GlobalExceptionsHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionsHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke
            (
            HttpContext context,
            DiplomaProjectContext dbContext
            )
        {

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExeptionAsync(context, ex, dbContext);
            }
        }

        private async Task HandleExeptionAsync
            (
            HttpContext context,
            Exception ex,
            DiplomaProjectContext dbContext
            )
        {
            var stackTrace = new StackTrace(ex, true);
            var frame = stackTrace.GetFrame(0);
            var method = frame.GetMethod();
            var methodName = method.Name;
            var className = method.DeclaringType.FullName;
            var lineNumber = frame.GetFileLineNumber();

            Console.WriteLine($"Exception in {className}.{methodName} at line {lineNumber}: {ex.Message}");


            var response = context.Response;

            switch (ex)
            {
                case UnauthorizedUserException:
                    await GenerateUserFaultResponse(response, ex, 401);
                    break;

                //========================================================================================
                //Domain Entities Exceptions
                case UrlException:
                    await GenerateUserFaultResponse(response, ex, 400);
                    break;
                case UserProblemException:
                    await GenerateUserFaultResponse(response, ex, 400);
                    break;

                //========================================================================================
                //Domain ValueObjects Exceptions
                case EmailException:
                    await GenerateUserFaultResponse(response, ex, 400);
                    break;
                case UrlTypeException:
                    await GenerateUserFaultResponse(response, ex, 400);
                    break;
                case UserProblemStatusException:
                    await GenerateUserFaultResponse(response, ex, 400);
                    break;

                //========================================================================================
                //App Exceptions
                default:
                    await GenerateAppFaultResponse(response, ex, dbContext);
                    break;
            }
        }

        private async Task GenerateUserFaultResponse
            (
            HttpResponse response,
            Exception ex,
            int statusCode
            )
        {
            response.ContentType = "application/json";
            response.StatusCode = statusCode;

            var errorResponse = new Application.Shared.DTOs.Response.Response
            {
                Status = EnumResponseStatus.UserFault,
                Message = ex.Message
            };

            var result = JsonSerializer.Serialize(errorResponse);
            await response.WriteAsync(result);
        }

        private async Task GenerateAppFaultResponse
            (
            HttpResponse response,
            Exception ex,
            DiplomaProjectContext dbContext
            )
        {
            switch (ex)
            {
                case InfrastructureLayerException:
                    Console.WriteLine(ex.Message);
                    break;
            }

            response.ContentType = "application/json";
            response.StatusCode = 500;

            var errorResponse = new ResponseAppException
            {
                Status = EnumResponseStatus.AppFault,
                Message = ex.Message,
                ProblemId = Guid.NewGuid()
            };

            var result = JsonSerializer.Serialize(errorResponse);
            await response.WriteAsync(result);
        }
    }
}
