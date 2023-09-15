
using System.Net;
using System.Text.Json;
using Task_Management_Service.Domain;

namespace Task_Management_Service.Infrastructure;
public class AppExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public AppExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);

       if (context.Response.StatusCode >= 400)
        {
            var error = new AppException(new[]{"An error occurred"},"SERVICE_ERROR",500);
           
            switch (context.Response.StatusCode)
            {
                case (int)HttpStatusCode.BadRequest:
                    error = new AppException(new[]{"Bad Request"},"SERVICE_ERROR",400);
                    break;
                case (int)HttpStatusCode.Unauthorized:
                    error = new AppException(new[]{"Unauthorized"},"SERVICE_ERROR",401);
                    break;
                case (int)HttpStatusCode.Forbidden:
                    error = new AppException(new[]{"Forbidden"},"SERVICE_ERROR",403);
                    break;
                case (int)HttpStatusCode.NotFound:
                    error = new AppException(new[]{"NotFound"},"SERVICE_ERROR",404);
                    break;
                case (int)HttpStatusCode.MethodNotAllowed:
                    error = new AppException(new[]{"MethodNotAllowed"},"SERVICE_ERROR",405);
                    break;
                case (int)HttpStatusCode.RequestTimeout:
                    error = new AppException(new[]{"RequestTimeout"},"SERVICE_ERROR",408);
                    break;
                case (int)HttpStatusCode.Conflict:
                    error = new AppException(new[]{"Conflict"},"SERVICE_ERROR",409);
                    break;
                case (int)HttpStatusCode.Gone:
                    error = new AppException(new[]{"Gone"},"SERVICE_ERROR",410);
                    break;
                case (int)HttpStatusCode.InternalServerError:
                    error = new AppException(new[]{"InternalServerError"},"SERVICE_ERROR",500);
                    break;
                case (int)HttpStatusCode.ServiceUnavailable:
                    error = new AppException(new[]{"ServiceUnavailable"},"SERVICE_ERROR",503);
                    break;
                case (int)HttpStatusCode.NotImplemented:
                    error = new AppException(new[]{"NotImplemented"},"SERVICE_ERROR",501);
                    break;
                case (int)HttpStatusCode.HttpVersionNotSupported:
                    error = new AppException(new[]{"HttpVersionNotSupported"},"SERVICE_ERROR",505);
                    break;
                default:
                    break;
            }

            var json = JsonSerializer.Serialize(new AppExceptionResponse(error));

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(json);
        }
    
    }
}
