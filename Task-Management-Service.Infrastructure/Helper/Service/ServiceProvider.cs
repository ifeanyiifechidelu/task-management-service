using Task_Management_Service.Domain;
using Serilog;
namespace Task_Management_Service.Infrastructure;


public class ServiceProvider
{ 
    
    public static void MapConfiguration(IConfiguration _configuration)
    {
        Log.Information("Mapping service configuration from env file to global configuration object...");
        Service.Address=_configuration.GetSection("Service")["Address"];
        Service.Port=_configuration.GetSection("Service")["Port"];
        Service.Name=_configuration.GetSection("Service")["Name"];;
        Service.Mode=_configuration.GetSection("Service")["Mode"];
        Service.LogFileName=_configuration.GetSection("Service")["LogFileName"];
        Service.LogDirectory=_configuration.GetSection("Service")["LogDirectory"];
        Service.EventBusUrl=_configuration.GetSection("Service")["EventBusUrl"];
        Service.LaunchUrl=_configuration.GetSection("Service")["LaunchUrl"];  
        Service.TokenAudience = _configuration.GetSection("Service")["TokenAudience"];
        Service.TokenIssuer = _configuration.GetSection("Service")["TokenIssuer"];
        Service.TokenKey = _configuration.GetSection("Service")["TokenKey"];
        Service.TokenExpiry = _configuration.GetSection("Service")["TokenExpiry"];
        Service.StripeSecretKey=_configuration.GetSection("Service")["StripeSecretKey"];
        Service.MailFrom=_configuration.GetSection("Service")["MailFrom"];
        Service.DisplayName=_configuration.GetSection("Service")["DisplayName"];
        Service.UserName=_configuration.GetSection("Service")["UserName"];
        Service.Password=_configuration.GetSection("Service")["Password"];
        Service.MailHost=_configuration.GetSection("Service")["MailHost"];
        Service.MailPort=_configuration.GetSection("Service")["MailPort"];
        
    }
    
    public static bool CheckConfiguration(string envFilePath)
    {
        Log.Information("Reading service configuration from env file...");
      
        if (!File.Exists(envFilePath)) 
        {
            Log.Error("Env file does not exist");
            return false;
        }
        foreach (var line in File.ReadAllLines(envFilePath))
        {
            var parts = line.Split(new[] {'='},2,StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2) continue;
            Log.Information("Setting global environment variable" + parts[0]);
            Environment.SetEnvironmentVariable(parts[0], parts[1]);
        }
        return true;
    }

    
}