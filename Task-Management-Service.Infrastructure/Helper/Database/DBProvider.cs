using Task_Management_Service.Domain;
using MongoDB.Driver;
using Serilog;

namespace Task_Management_Service.Infrastructure;

public class DBProvider: IDBProvider
{ 
    readonly IConfiguration _configuration;
    readonly DBConnection _connection;
    readonly IMongoDatabase _database; // database instance
   
    public DBProvider(IConfiguration configuration, DBConnection connection)
    {
        Log.Information("Establishing connection");
        _connection = connection;
        Log.Information("Establishing configuration");
        _configuration = configuration;
        _configuration.GetSection(nameof(DBConnection)).Bind(_connection = connection); 
        Log.Information("Connecting to MongoDB database");
        var client = new MongoClient(_connection.ConnectionString); 

        // Check if we need to delete the database
        if (_connection.NewDB)
        {
            Log.Warning("Dropping existing database: {0}", _connection.DatabaseName);
            client.DropDatabase(_connection.DatabaseName);
        }

        Log.Information("Retrieving database");
        _database = client.GetDatabase(_connection.DatabaseName);  
        Log.Information("MongoDB database connection successfully established");
    }
    
    public IMongoDatabase Connect()
    {
            return _database;
    }

    public int GetPageLimit()
    {
        return _connection.PageLimit;
    }
}
