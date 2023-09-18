

using MongoDB.Bson.Serialization.Attributes;

namespace Task_Management_Service.Domain;
public class EmailResult
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
}