# CSharpDatabase
<small>This README was created with a little help from ChatGPT</small>

CSharpDatabase is an API and console application for managing information related to farming associations, crops, fields, and workers.

## Console Interface

### Login
The console application provides an interactive interface for managing the database.

- When logging in, the system registers the user in the database.
- The user can specify a database schema. If the schema does not exist, the system asks whether to create it.
- After the first login, the credentials are saved to simplify future logins.

### Console Commands
Once logged in, a console environment opens, allowing users to manage records in the database.

- Commands are available for inserting, removing, and updating records.
- Use the command `help` to view all available commands.

## Database Tables
The database schema is defined in `database.sql`. The following tables are included:

```
Crops              - name, price
Fields             - name, Association
Associations       - name
Workers            - name, surname, birthDate, fired
SowingRecords      - Field, Association, Worker, date
```
To view table details within the console, use the command:  `table-info`

## Program Structure
Each database table is represented by a corresponding class. These classes provide methods for managing records:

- `Update()`
- `Insert()`
- `Remove()`
- `GetById()`
- Additional record specific `GetBy` methods

The SQL database itself is represented by the `Database` class.  
To establish a connection, use the `DatabaseCredentials` class, which supports JSON serialization and deserialization.

### Code Examples
Connecting to the database
```
var credentials = new DatabaseCredentials("url", "database name", "username", "password");

using(var database = new Database(credentials))
{

}
```

Inserting and removing records
```
using(var database = new Database(credentials))
{
	Crop crop = new Crop("crop name", 1.3f, database);
	crop.Insert(); // inserts the crop into db
	
	crop.Remove(); // remove the crop
}
```

Starting a console environment
```
DatabaseConsole.Run();
```
