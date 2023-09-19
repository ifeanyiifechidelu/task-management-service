using Microsoft.AspNetCore.Mvc;
using Task_Management_Service.Domain;
using Serilog;
using Task_Management_Service.Infrastructure;
using Task_Management_Service.Api.Extensions;
using serviceProvider = Task_Management_Service.Infrastructure.ServiceProvider;
using Task_Management_Service.Api;

var builder = WebApplication.CreateBuilder(args);

// Check for service configuration
if (!serviceProvider.CheckConfiguration(@"../Task-Management-Service.env")) return;

// Add environment variables to the global config
builder.Configuration.AddEnvironmentVariables();

// Map configuration to global class
serviceProvider.MapConfiguration(builder.Configuration);

// Suppress automatic model validation
builder.Services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });

// Add Serilog
builder.Host.AddSerilog();

// Register services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHeaderPropagation(options => options.Headers.Add("X-Correlation-Id"));
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddSingleton<serviceProvider>();

builder.Services.AddTransient<IUserRepository,UserRepository>();
builder.Services.AddTransient<IUserService,UserService>();
builder.Services.AddTransient<IUserValidationService,UserValidationService>();
builder.Services.AddTransient<IProjectRepository,ProjectRepository>();
builder.Services.AddTransient<IProjectService,ProjectService>();
builder.Services.AddTransient<IProjectValidationService,ProjectValidationService>();
builder.Services.AddTransient<ITaskRepository,TaskRepository>();
builder.Services.AddTransient<ITaskService,TaskService>();
builder.Services.AddTransient<ITaskValidationService,TaskValidationService>();
builder.Services.AddTransient<INotificationRepository,NotificationRepository>();
builder.Services.AddTransient<INotificationService,NotificationService>();
builder.Services.AddTransient<INotificationValidationService,NotificationValidationService>();
builder.Services.AddTransient<ISendNotificationService,SendNotificationService>();
builder.Services.AddTransient<IEmailService,EmailService>();

builder.Services.AddStackExchangeRedisCache(options => options.Configuration = builder.Configuration.GetSection("Redis")["RedisServer"]);


builder.Services.AddTransient<ICacheProvider, CacheProvider>();
builder.Services.AddSingleton<IDBProvider, DBProvider>();
builder.Services.AddSingleton<DBConnection>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

// Define the service address
builder.WebHost.UseUrls($"{Service.Address}:{Service.Port}");
var app = builder.Build();



// Middleware pipeline
app.UseMiddleware<AppExceptionHandlerMiddleware>();
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();  // Consider placing this earlier in the pipeline
app.UseRouting();
app.UseCors("AllowAll");
app.UseHeaderPropagation();
app.UseAuthentication();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();