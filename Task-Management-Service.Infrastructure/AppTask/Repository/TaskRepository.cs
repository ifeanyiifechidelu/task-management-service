using Task_Management_Service.Domain;
using MongoDB.Driver;
using Serilog;
using MongoDB.Bson;


namespace Task_Management_Service.Infrastructure;
public class TaskRepository : ITaskRepository
{
    readonly IDBProvider _dbProvider;
    readonly IMongoCollection<ServiceTask> _task;


    public TaskRepository(IDBProvider dbProvider)
    {

        _dbProvider = dbProvider;
        _task = _dbProvider.Connect().GetCollection<ServiceTask>(nameof(ServiceTask).ToLower());

    }
    public async Task<string> CreateTask(ServiceTask task)
    {
        try
        {
            Log.Information("Inserting Task Data");
            await _task.InsertOneAsync(task);
            Log.Information("Data Inserted");
            return task.Reference;
        }
        catch (Exception e)
        {
            Log.Error("Error Creating Task: {0}", e.Message);
            throw DatabaseExceptionHandler.HandleException(e);
        }
    }
    public async Task<string> UpdateTask(string reference, ServiceTask task)
    {
        try
        {

            Log.Information("Updating Data");
            await _task.ReplaceOneAsync(task => task.Reference == reference, task);
            Log.Information("Data Updated");
            return reference;
        }
        catch (Exception e)
        {
            Log.Error("Error Updating Task: {0}", e.Message);
            throw DatabaseExceptionHandler.HandleException(e);
        }
    }

    public async Task<string> DeleteTask(string reference)
    {
        try
        {

            Log.Information("Deleting data");
            var result = await _task.DeleteOneAsync(data => data.Reference == reference);
            Log.Information("Data Deleted");
            return reference;
        }
        catch (Exception e)
        {
            Log.Error("Error Deleting Task: {0}", e.Message);
            throw DatabaseExceptionHandler.HandleException(e);
        }
    }
    public async Task<ServiceTask> GetTaskByReference(string reference)
    {
        try
        {
            Log.Information("Getting data by reference {0}", reference);

            var result = await _task.Find(task => task.Reference == reference).FirstOrDefaultAsync();
            return result;
        }
        catch (Exception e)
        {
            Log.Error("Error Getting Task: {0}", e.Message);
            throw DatabaseExceptionHandler.HandleException(e);
        }
    }

    public async Task<List<ServiceTask>> GetTasksByStatus(string status)
    {
        try
        {
            Log.Information("Searching task by status: {0}", status);

            var filter = Builders<ServiceTask>.Filter.Eq(task => task.Status, status);

            return await _task.Find(filter).ToListAsync();
        }
        catch (Exception e)
        {
            Log.Error("Error retrieving task by status: {0}", e.Message);
            throw DatabaseExceptionHandler.HandleException(e);
        }
    }

    public async Task<List<ServiceTask>> GetTasksByPriority(string priority)
    {
        try
        {
            Log.Information("Searching task by priority: {0}", priority);

            var filter = Builders<ServiceTask>.Filter.Eq(task => task.Priority, priority);

            return await _task.Find(filter).ToListAsync();
        }
        catch (Exception e)
        {
            Log.Error("Error retrieving task by priority: {0}", e.Message);
            throw DatabaseExceptionHandler.HandleException(e);
        }
    }

    public async Task<List<ServiceTask>> GetTasksDueByCurrentWeek(DateTime weekStartDate, DateTime weekEndDate)
    {
        try
        {

            Log.Information("Searching tasks due for the current week.");

            var filter = Builders<ServiceTask>.Filter.And(
                Builders<ServiceTask>.Filter.Gte(task => task.DueDate, weekStartDate),
                Builders<ServiceTask>.Filter.Lte(task => task.DueDate, weekEndDate)
            );

            return await _task.Find(filter).ToListAsync();
        }
        catch (Exception e)
        {
            Log.Error("Error retrieving task due for the current week: {0}", e.Message);
            throw DatabaseExceptionHandler.HandleException(e);
        }
    }

    public async Task<List<ServiceTask>> GetTasksByUserReference(string userReference)
    {
        try
        {
            Log.Information("Searching task by user reference: {0}", userReference);

            var filter = Builders<ServiceTask>.Filter.Eq(task => task.UserReference, userReference);

            return await _task.Find(filter).ToListAsync();
        }
        catch (Exception e)
        {
            Log.Error("Error retrieving task by user reference: {0}", e.Message);
            throw DatabaseExceptionHandler.HandleException(e);
        }
    }

    public async Task<List<ServiceTask>> GetTasksByProjectReference(string projectReference)
    {
        try
        {
            Log.Information("Searching task by user reference: {0}", projectReference);

            var filter = Builders<ServiceTask>.Filter.Eq(task => task.ProjectReference, projectReference);

            return await _task.Find(filter).ToListAsync();
        }
        catch (Exception e)
        {
            Log.Error("Error retrieving task by user reference: {0}", e.Message);
            throw DatabaseExceptionHandler.HandleException(e);
        }
    }

    public async Task<List<ServiceTask>> GetTaskList(int page)
    {
        try
        {
            Log.Information("Getting data by page {0}", page);

            return await _task.Find(task => true).Skip((page - 1) * _dbProvider.GetPageLimit())
            .Limit(_dbProvider.GetPageLimit()).ToListAsync();

        }
        catch (Exception e)
        {
            Log.Error("Error Getting Task: {0}", e.Message);
            throw DatabaseExceptionHandler.HandleException(e);
        }
    }
    public async Task<List<ServiceTask>> SearchTaskList(int page, string title)
    {
        try
        {
            Log.Information("Searching data by page {0} and title {1}", page, title);



            var filterBuilder = Builders<ServiceTask>.Filter;
            var titleFilter = filterBuilder.Regex(task => task.Title, new BsonRegularExpression($"/{title}/"));

            var filter = titleFilter;

            return await _task.Find(filter).Skip((page - 1) * _dbProvider.GetPageLimit())
            .Limit(_dbProvider.GetPageLimit()).ToListAsync();

        }
        catch (Exception e)
        {
            Log.Error("Error Searching Task: {0}", e.Message);
            throw DatabaseExceptionHandler.HandleException(e);
        }
    }
}