using System;
using MongoDB.Driver;

namespace Task_Management_Service.Domain
{
    public class DatabaseException : AppException
    {
        public int ErrorCode { get; set; }

        public DatabaseException(string error, int errorCode = 0) : base(new[] { error }, "DatabaseError", 500) 
        {
            this.ErrorCode = errorCode;
        }
    }

    public static class DatabaseExceptionHandler
    {
        public static AppException HandleException(Exception exception)
        {
            if (exception is MongoException mongoException)
            {
                return HandleMongoException(mongoException);
            }
            return new DatabaseException(exception.Message);
        }

private static AppException HandleMongoException(MongoException exception)
{
    return exception switch
    {

        MongoAuthenticationException authEx => 
            new DatabaseException($"Authentication failed. Error: {authEx.Message}"),
        MongoExecutionTimeoutException timeoutEx => 
            new DatabaseException($"A timeout occurred. Error: {timeoutEx.Message}"),
        MongoWriteConcernException writeConcernEx => 
            new DatabaseException($"Write concern error. Code: {writeConcernEx.Code}. Error: {writeConcernEx.Message}"),
        MongoWriteException writeEx when writeEx.WriteError.Category == ServerErrorCategory.DuplicateKey => 
            new ConflictException($"Duplicate record found. Error: {writeEx.Message}"),
        MongoConnectionException connEx => 
            new DatabaseException($"Failed to connect to MongoDB. Error: {connEx.Message}"),
        MongoCommandException cmdEx => 
            new DatabaseException($"MongoDB command error. Code: {cmdEx.Code}. Error: {cmdEx.Message}"),
        MongoCursorNotFoundException cursorNotFoundEx => 
            new DatabaseException($"Cursor not found on the server. Error: {cursorNotFoundEx.Message}"),
        MongoQueryException queryEx => 
            new DatabaseException($"Error executing a query operation. Error: {queryEx.Message}"),
        MongoInternalException internalEx => 
            new DatabaseException($"An internal MongoDB driver error occurred. Error: {internalEx.Message}"),
            
        _ => new DatabaseException($"Unknown MongoDB error. Error: {exception.Message}"),
    };
}

    }
}
