using Microsoft.Data.SqlClient;
using System.Text.Json;
using System.Text.Json.Serialization;

public class DatabaseCredentials
{
    private string username;
    private string password;
    private string databaseName;
    private string url;

    public string Username { get => username; }
    public string Password { get => password; }
    public string DatabaseName { get => databaseName; }
    public string Url { get => url; }
    [JsonIgnore]
    public SqlConnectionStringBuilder ConnectionBuilderNoDB { 
        get {
            return new SqlConnectionStringBuilder
            {
                DataSource = url,
                UserID = username,
                Password = password,
                TrustServerCertificate = true,
            };
        }
    }
    [JsonIgnore]
    public SqlConnectionStringBuilder ConnectionBuilder { 
        get {
            return new SqlConnectionStringBuilder
            {
                DataSource = url,
                UserID = username,
                Password = password,
                InitialCatalog = databaseName,
                TrustServerCertificate = true,
            };
        }
    }

    public DatabaseCredentials(string url, string databaseName, string username, string password) 
    {
        this.username = username;
        this.password = password;
        this.databaseName = databaseName;
        this.url = url;
    }

    public static DatabaseCredentials FromJson(string json)
    {
        DatabaseCredentials? result = JsonSerializer.Deserialize<DatabaseCredentials>(json);
        if(result == null) throw new JsonException("Failed to parse credentials");
        return result;
    }

    public string ToJson()
    {
        return JsonSerializer.Serialize(this);
    }

    public override string ToString()
    {
        return $"DatabaseCredentials{{url={url}, databaseName={databaseName}, username={username}, password={password}}}";
    }
}
