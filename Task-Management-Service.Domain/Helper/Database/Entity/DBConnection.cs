
namespace Task_Management_Service.Domain;
public class DBConnection
   {
      public string ConnectionString { get; set; }
      public string DatabaseName { get; set; }
      
      public int PageLimit { get; set; }
      public  bool NewDB {get;set;}
}
