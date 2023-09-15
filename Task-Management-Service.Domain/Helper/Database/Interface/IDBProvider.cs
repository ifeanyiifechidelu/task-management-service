
using MongoDB.Driver;
namespace Task_Management_Service.Domain;
public interface IDBProvider
   {
      public IMongoDatabase Connect();
      public int GetPageLimit();
   }