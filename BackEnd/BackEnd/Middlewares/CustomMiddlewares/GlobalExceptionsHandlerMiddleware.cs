using Application.Database;
using Application.Shared.DTOs.Response;
using Application.Shared.Exceptions.UserExceptions;
using Domain.Exceptions.UserExceptions.ValueObjectsExceptions;
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
            Exception exception,
            DiplomaProjectContext dbContext
            )
        {
            var response = context.Response;


            switch (exception)
            {
                case ApartmentNumberException:
                    await GenerateUserFaultResponse(response, exception, 400);
                    break;
                case BuildingNumberException:
                    await GenerateUserFaultResponse(response, exception, 400);
                    break;
                case CommentEvaluationException:
                    await GenerateUserFaultResponse(response, exception, 400);
                    break;
                case CommentTypeException:
                    await GenerateUserFaultResponse(response, exception, 400);
                    break;
                case DatabaseBoolException:
                    await GenerateUserFaultResponse(response, exception, 400);
                    break;
                case EmailException:
                    await GenerateUserFaultResponse(response, exception, 400);
                    break;
                case MoneyException:
                    await GenerateUserFaultResponse(response, exception, 400);
                    break;
                case PhoneNumberException:
                    await GenerateUserFaultResponse(response, exception, 400);
                    break;
                case RegonException:
                    await GenerateUserFaultResponse(response, exception, 400);
                    break;
                case UrlSegmentException:
                    await GenerateUserFaultResponse(response, exception, 400);
                    break;
                case UrlException:
                    await GenerateUserFaultResponse(response, exception, 400);
                    break;
                case UrlTypeException:
                    await GenerateUserFaultResponse(response, exception, 400);
                    break;
                case UserProblemStatusException:
                    await GenerateUserFaultResponse(response, exception, 400);
                    break;
                case ZipCodeException:
                    await GenerateUserFaultResponse(response, exception, 400);
                    break;
                case UnauthorizedUserException:
                    await GenerateUserFaultResponse(response, exception, 401);
                    break;
                default:
                    await GenerateAppFaultResponse(response, exception, dbContext);
                    break;
            }
        }

        private async Task GenerateUserFaultResponse
            (
            HttpResponse response,
            Exception exception,
            int statusCode
            )
        {
            response.ContentType = "application/json";
            response.StatusCode = statusCode;

            var errorResponse = new Application.Shared.DTOs.Response.Response
            {
                Status = EnumResponseStatus.UserFault,
                Message = exception.Message
            };

            var result = JsonSerializer.Serialize(errorResponse);
            await response.WriteAsync(result);
        }

        private async Task GenerateAppFaultResponse
            (
            HttpResponse response,
            Exception exception,
            DiplomaProjectContext dbContext
            )
        {
            response.ContentType = "application/json";
            response.StatusCode = 500;

            var errorResponse = new Application.Shared.DTOs.Response.AppExceptionResponse
            {
                Status = EnumResponseStatus.AppFault,
                Message = exception.Message,
                Id = Guid.NewGuid()
            };

            var result = JsonSerializer.Serialize(errorResponse);
            await response.WriteAsync(result);
        }
    }
}
