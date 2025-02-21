DatabaseConsole.Run();

var credentials = new DatabaseCredentials("url", "database name", "username", "password");

using(var database = new Database(credentials))
{

}
