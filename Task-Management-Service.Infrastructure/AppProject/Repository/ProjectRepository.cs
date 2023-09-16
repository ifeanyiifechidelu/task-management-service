using Task_Management_Service.Domain;
using MongoDB.Driver;
using Serilog;
using MongoDB.Bson;


namespace Task_Management_Service.Infrastructure;
public class ProjectRepository : IProjectRepository
{
    readonly IDBProvider _dbProvider;
    readonly IMongoCollection<Project> _project;


    public ProjectRepository(IDBProvider dbProvider)
    {

        _dbProvider = dbProvider;
        _project = _dbProvider.Connect().GetCollection<Project>(nameof(Project).ToLower());

    }
    public async Task<string> CreateProject(Project project)
    {
        try
        {
            Log.Information("Inserting Project Data");
            await _project.InsertOneAsync(project);
            Log.Information("Data Inserted");
            return project.Reference;
        }
        catch (Exception e)
        {
            Log.Error("Error Creating Project: {0}", e.Message);
            throw DatabaseExceptionHandler.HandleException(e);
        }
    }
    public async Task<string> UpdateProject(string reference, Project project)
    {
        try
        {

            Log.Information("Updating Data");
            await _project.ReplaceOneAsync(project => project.Reference == reference, project);
            Log.Information("Data Updated");
            return reference;
        }
        catch (Exception e)
        {
            Log.Error("Error Updating Project: {0}", e.Message);
            throw DatabaseExceptionHandler.HandleException(e);
        }
    }
    public async Task<string> DeleteProject(string reference)
    {
        try
        {

            Log.Information("Deleting data");
            var result = await _project.DeleteOneAsync(data => data.Reference == reference);
            Log.Information("Data Deleted");
            return reference;
        }
        catch (Exception e)
        {
            Log.Error("Error Deleting Project: {0}", e.Message);
            throw DatabaseExceptionHandler.HandleException(e);
        }
    }
    public async Task<Project> GetProjectByReference(string reference)
    {
        try
        {
            Log.Information("Getting data by reference {0}", reference);

            var result = await _project.Find(project => project.Reference == reference).FirstOrDefaultAsync();
            return result;
        }
        catch (Exception e)
        {
            Log.Error("Error Getting Project: {0}", e.Message);
            throw DatabaseExceptionHandler.HandleException(e);
        }
    }

    public async Task<List<Project>> GetProjectList(int page)
    {
        try
        {
            Log.Information("Getting data by page {0}", page);

            return await _project.Find(project => true).Skip((page - 1) * _dbProvider.GetPageLimit())
            .Limit(_dbProvider.GetPageLimit()).ToListAsync();

        }
        catch (Exception e)
        {
            Log.Error("Error Getting Project: {0}", e.Message);
            throw DatabaseExceptionHandler.HandleException(e);
        }
    }
    public async Task<List<Project>> SearchProjectList(int page, string name)
    {
        try
        {
            Log.Information("Searching data by page {0}", page);



            var filterBuilder = Builders<Project>.Filter;
            var nameFilter = filterBuilder.Regex(project => project.Name, new BsonRegularExpression($"/{name}/"));

            var filter = nameFilter;

            return await _project.Find(filter).Skip((page - 1) * _dbProvider.GetPageLimit())
            .Limit(_dbProvider.GetPageLimit()).ToListAsync();

        }
        catch (Exception e)
        {
            Log.Error("Error Searching Project: {0}", e.Message);
            throw DatabaseExceptionHandler.HandleException(e);
        }
    }
}