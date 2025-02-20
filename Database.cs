using Microsoft.Data.SqlClient;

public class Database : IDisposable
{
    public static string DATABASE_SQL = "database.sql";
    private SqlConnection connection;

    public SqlConnection Connection {get => connection; }

    public Database(DatabaseCredentials credentials, string databaseName)
    {
        var conBuilder = credentials.ConnectionBuilder;
        conBuilder.InitialCatalog = databaseName;
        
        connection = new SqlConnection(conBuilder.ConnectionString);
        connection.Open();
    }

    public static void Create(DatabaseCredentials credentials, string databaseName)
    {
        string sql = File.ReadAllText(DATABASE_SQL);
        
        var conBuilder = credentials.ConnectionBuilder;
        using(var connection = new SqlConnection(conBuilder.ConnectionString)) {
            connection.Open();
           
            var dbCommand = new SqlCommand($"create database [{databaseName}]", connection);
            dbCommand.ExecuteNonQuery(); 
           
        }
        
        conBuilder.InitialCatalog = databaseName;
        using(var connection = new SqlConnection(conBuilder.ConnectionString)) {
            connection.Open();
            
            var tableCommand= new SqlCommand(sql, connection);
            tableCommand.ExecuteNonQuery();
           
        }
    }
    
    public static bool Exists(DatabaseCredentials credentials, string databaseName) 
    {
        var conBuilder = credentials.ConnectionBuilder;
        
        bool exists = true;
        using (var connection = new SqlConnection(conBuilder.ConnectionString))
        {
            connection.Open();
            using var command = new SqlCommand("SELECT name FROM sys.databases WHERE name = @name", connection);
            command.Parameters.AddWithValue("@name", databaseName);

            var reader = command.ExecuteScalar(); 
            if(reader == null) exists = false;
        }

        return exists;
    }
    
    public void Dispose()
    {
        connection.Dispose();
    }

}
