using Microsoft.Data.SqlClient;

public class Worker : SqlItem 
{
    private int id;
    private Database database;
    private string name;
    private string surname;
    private DateTime birthDate;
    private Association association;
    private bool fired;
    
    public int Id { get => id; }
    public Database Database { get => database; }
    public string Name { get => name; }
    public string Surname { get => surname; }
    public DateTime BirthDate { get => birthDate; }
    public Association Association { get => association; }
    public bool Fired { get => fired; }

    
   public Worker(string name, string surname, DateTime birthDate, Association association, bool fired, Database database)
   {
        this.id = -1;
        this.database = database;
        this.name = name;
        this.surname = surname;
        this.birthDate = birthDate;
        this.association = association;
        this.fired = fired;
   }

   public Worker(int id, string name, string surname, DateTime birthDate, Association association, bool fired, Database database)
   {
        this.id = id;
        this.database = database;
        this.name = name;
        this.surname = surname;
        this.birthDate = birthDate;
        this.association = association;
        this.fired = fired;
   }

   public void Insert()
   {
        if(id != - 1) throw new DatabaseException("Worker is already in database");
        
        using var command = new SqlCommand("INSERT INTO Workers (name, surname, birthDate, idAssociation, fired) VALUES (@name, @surname, @birthDate, @idAssociation, @fired)", database.Connection);
        command.Parameters.AddWithValue("@name", name);
        command.Parameters.AddWithValue("@surname", surname);
        command.Parameters.AddWithValue("@birthDate", birthDate);
        command.Parameters.AddWithValue("@idAssociation", association.Id);
        command.Parameters.AddWithValue("@fired", fired);
        command.ExecuteNonQuery(); 
        
        // Get the id 
        using var command2 = new SqlCommand("SELECT TOP 1 idWorker FROM idWorker ORDER BY idWorker DESC", database.Connection);
        var result = command2.ExecuteScalar();
        id = Convert.ToInt32(result); 
    }

   public void Remove()
   {
        if(id == -1) throw new DatabaseException("Worker not in database");
        
        using var command = new SqlCommand("DELETE FROM Workers WHERE idWorker = @idWorker", database.Connection);
        command.Parameters.AddWithValue("@idWorker", id);
        command.ExecuteNonQuery();
        id = -1;
   }

   public static Worker GetById(int id, Database database)
   {
        using var command = new SqlCommand("SELECT idWorker, name, surname, birthDate, idAssociation, fired FROM Workers WHERE idWorker = @id", database.Connection);
        command.Parameters.AddWithValue("@id", id);
        var reader = command.ExecuteReader();
        if(reader.Read())
        {
            return new Worker(reader.GetInt32(0),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetDateTime(4),
                    Association.GetById(reader.GetInt32(5), database),
                    reader.GetBoolean(6), 
                    database); 
        }
        else
        {
            throw new DatabaseException($"Field with id {id} not found"); 
        }
   }

   public static Worker GetByName(string name, string surname, Database database)
   {
        using var command = new SqlCommand("SELECT idWorker, name, surname, birthDate, idAssociation, fired FROM Workers WHERE name = @name AND surname = @surname", database.Connection);
        command.Parameters.AddWithValue("@name", name);
        command.Parameters.AddWithValue("@surname", surname);
        var reader = command.ExecuteReader();
        if(reader.Read())
        {
            return new Worker(reader.GetInt32(0),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetDateTime(4),
                    Association.GetById(reader.GetInt32(5), database),
                    reader.GetBoolean(6), 
                    database); 
        }
        else
        {
            throw new DatabaseException($"Field with name {name} {surname} not found"); 
        }
   }

    public static Worker[] GetAll(Database database)
    {
        List<Worker> list = new List<Worker>();
        using var command = new SqlCommand("SELECT idWorker, name, surname, birthDate, idAssociation, fired FROM Workers WHERE name = @name AND surname = @surname ORDER BY idWorker DESC",
                database.Connection);
        using var reader = command.ExecuteReader();
        
        while(reader.Read())
        {
            list.Add(new Worker(reader.GetInt32(0),
                        reader.GetString(2),
                        reader.GetString(3),
                        reader.GetDateTime(4),
                        Association.GetById(reader.GetInt32(5), database),
                        reader.GetBoolean(6),
                        database)); 
        }
        
        return list.ToArray();
    }

    public static Worker[] GetAll(Database database, int max)
    {
        List<Worker> list = new List<Worker>();
        using var command = new SqlCommand($"SELECT TOP {max} idWorker, name, surname, birthDate, idAssociation, fired FROM Workers WHERE name = @name AND surname = @surname ORDER BY idWorker DESC",
                database.Connection);
        using var reader = command.ExecuteReader();
        
        while(reader.Read())
        {
            list.Add(new Worker(reader.GetInt32(0),
                        reader.GetString(2),
                        reader.GetString(3),
                        reader.GetDateTime(4),
                        Association.GetById(reader.GetInt32(5), database),
                        reader.GetBoolean(6),
                        database)); 
        }
        
        return list.ToArray();
    }
}

