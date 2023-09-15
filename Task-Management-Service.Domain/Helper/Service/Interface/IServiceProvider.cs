namespace Task_Management_Service.Domain;

public interface IServiceProvider
{
    void MapConfig();
    void ReadConfig(string envFilePath);
    
}
    
