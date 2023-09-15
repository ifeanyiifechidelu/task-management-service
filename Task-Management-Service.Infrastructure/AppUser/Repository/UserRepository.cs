using Task_Management_Service.Domain;
using MongoDB.Driver;
using Serilog;
using MongoDB.Bson;


namespace Task_Management_Service.Infrastructure;
public class UserRepository : IUserRepository
{
    readonly IDBProvider _dbProvider;
    readonly IMongoCollection<User> _user;


    public UserRepository(IDBProvider dbProvider)
    {

        _dbProvider = dbProvider;
        _user = _dbProvider.Connect().GetCollection<User>(nameof(User).ToLower());

    }
    public async Task<string> CreateUser(User user)
    {
        try
        {
            Log.Information("Inserting User Data");
            await _user.InsertOneAsync(user);
            Log.Information("Data Inserted");
            return user.Reference;
        }
        catch (Exception e)
        {
            Log.Error("Error Creating User: {0}", e.Message);
            throw DatabaseExceptionHandler.HandleException(e);
        }
    }
    public async Task<string> UpdateUser(string reference, User user)
    {
        try
        {

            Log.Information("Updating Data");
            await _user.ReplaceOneAsync(user => user.Reference == reference, user);
            Log.Information("Data Updated");
            return reference;
        }
        catch (Exception e)
        {
            Log.Error("Error Updating User: {0}", e.Message);
            throw DatabaseExceptionHandler.HandleException(e);
        }
    }
    public async Task<string> DeleteUser(string reference)
    {
        try
        {

            Log.Information("Deleting data");
            var result = await _user.DeleteOneAsync(data => data.Reference == reference);
            Log.Information("Data Deleted");
            return reference;
        }
        catch (Exception e)
        {
            Log.Error("Error Deleting User: {0}", e.Message);
            throw DatabaseExceptionHandler.HandleException(e);
        }
    }
    public async Task<User> GetUserByReference(string reference)
    {
        try
        {
            Log.Information("Getting data by reference {0}", reference);

            var result = await _user.Find(user => user.Reference == reference).FirstOrDefaultAsync();
            return result;
        }
        catch (Exception e)
        {
            Log.Error("Error Getting User: {0}", e.Message);
            throw DatabaseExceptionHandler.HandleException(e);
        }
    }

    public async Task<User> GetUserByEmail(string email)
    {
        try
        {
            Log.Information("Searching user by username: {0}", email);

            var filter = Builders<User>.Filter.Eq(user => user.Email, email);
            
            return await _user.Find(filter).FirstOrDefaultAsync();
        }
        catch (Exception e)
        {
            Log.Error("Error retrieving user by username: {0}", e.Message);
            throw DatabaseExceptionHandler.HandleException(e);
        }
    }

    public async Task<List<User>> GetUserList(int page)
    {
        try
        {
            Log.Information("Getting data by page {0}", page);

            return await _user.Find(user => true).Skip((page - 1) * _dbProvider.GetPageLimit())
            .Limit(_dbProvider.GetPageLimit()).ToListAsync();

        }
        catch (Exception e)
        {
            Log.Error("Error Getting User: {0}", e.Message);
            throw DatabaseExceptionHandler.HandleException(e);
        }
    }
    public async Task<List<User>> SearchUserList(int page, string fullname)
    {
        try
        {
            Log.Information("Searching data by page {0}", page);



            var filterBuilder = Builders<User>.Filter;
            var fullnameFilter = filterBuilder.Regex(user => user.FullName, new BsonRegularExpression($"/{fullname}/"));

            var filter = fullnameFilter;

            return await _user.Find(filter).Skip((page - 1) * _dbProvider.GetPageLimit())
            .Limit(_dbProvider.GetPageLimit()).ToListAsync();

        }
        catch (Exception e)
        {
            Log.Error("Error Searching User: {0}", e.Message);
            throw DatabaseExceptionHandler.HandleException(e);
        }
    }
}