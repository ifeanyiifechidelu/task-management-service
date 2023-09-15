
namespace Task_Management_Service.Api.Extensions;

public static class CorsExtensions
{
    public static IApplicationBuilder UseCustomCors(this IApplicationBuilder app)
    {
        app.UseCors(builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });

        return app;
    }
}