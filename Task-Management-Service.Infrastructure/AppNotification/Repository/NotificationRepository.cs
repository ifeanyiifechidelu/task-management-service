using Task_Management_Service.Domain;
using MongoDB.Driver;
using Serilog;
using MongoDB.Bson;


namespace Task_Management_Service.Infrastructure;
public class NotificationRepository : INotificationRepository
{
    readonly IDBProvider _dbProvider;
    readonly IMongoCollection<Notification> _notification;


    public NotificationRepository(IDBProvider dbProvider)
    {

        _dbProvider = dbProvider;
        _notification = _dbProvider.Connect().GetCollection<Notification>(nameof(Notification).ToLower());

    }
    public async Task<string> CreateNotification(Notification notification)
    {
        try
        {
            Log.Information("Inserting Notification Data");
            await _notification.InsertOneAsync(notification);
            Log.Information("Data Inserted");
            return notification.Reference;
        }
        catch (Exception e)
        {
            Log.Error("Error Creating Notification: {0}", e.Message);
            throw DatabaseExceptionHandler.HandleException(e);
        }
    }
    public async Task<string> UpdateNotification(string reference, Notification notification)
    {
        try
        {

            Log.Information("Updating Data");
            await _notification.ReplaceOneAsync(notification => notification.Reference == reference, notification);
            Log.Information("Data Updated");
            return reference;
        }
        catch (Exception e)
        {
            Log.Error("Error Updating Notification: {0}", e.Message);
            throw DatabaseExceptionHandler.HandleException(e);
        }
    }
    public async Task<string> DeleteNotification(string reference)
    {
        try
        {

            Log.Information("Deleting data");
            var result = await _notification.DeleteOneAsync(data => data.Reference == reference);
            Log.Information("Data Deleted");
            return reference;
        }
        catch (Exception e)
        {
            Log.Error("Error Deleting Notification: {0}", e.Message);
            throw DatabaseExceptionHandler.HandleException(e);
        }
    }
    public async Task<Notification> GetNotificationByReference(string reference)
    {
        try
        {
            Log.Information("Getting data by reference {0}", reference);

            var result = await _notification.Find(notification => notification.Reference == reference).FirstOrDefaultAsync();
            return result;
        }
        catch (Exception e)
        {
            Log.Error("Error Getting Notification: {0}", e.Message);
            throw DatabaseExceptionHandler.HandleException(e);
        }
    }

    // public async Task<Notification> GetNotificationByType(string type)
    // {
    //     try
    //     {
    //         Log.Information("Searching notification by notificationname: {0}", email);

    //         var filter = Builders<Notification>.Filter.Eq(notification => notification.Email, email);
            
    //         return await _notification.Find(filter).FirstOrDefaultAsync();
    //     }
    //     catch (Exception e)
    //     {
    //         Log.Error("Error retrieving notification by notificationname: {0}", e.Message);
    //         throw DatabaseExceptionHandler.HandleException(e);
    //     }
    // }

    public async Task<List<Notification>> GetNotificationList(int page)
    {
        try
        {
            Log.Information("Getting data by page {0}", page);

            return await _notification.Find(notification => true).Skip((page - 1) * _dbProvider.GetPageLimit())
            .Limit(_dbProvider.GetPageLimit()).ToListAsync();

        }
        catch (Exception e)
        {
            Log.Error("Error Getting Notification: {0}", e.Message);
            throw DatabaseExceptionHandler.HandleException(e);
        }
    }
    public async Task<List<Notification>> SearchNotificationList(int page, string type)
    {
        try
        {
            Log.Information("Searching data by page {0}", page);



            var filterBuilder = Builders<Notification>.Filter;
            var typeFilter = filterBuilder.Regex(notification => notification.Type, new BsonRegularExpression($"/{type}/"));

            var filter = typeFilter;

            return await _notification.Find(filter).Skip((page - 1) * _dbProvider.GetPageLimit())
            .Limit(_dbProvider.GetPageLimit()).ToListAsync();

        }
        catch (Exception e)
        {
            Log.Error("Error Searching Notification: {0}", e.Message);
            throw DatabaseExceptionHandler.HandleException(e);
        }
    }
}