using Microsoft.Data.SqlClient;

/// <summary>
/// Represents a worker stored in a database.
/// </summary>
public class Worker : SqlItem 
{
    private int id;
    private Database database;
    private string name;
    private string surname; private DateTime birthDate;
    private int associationId;
    private bool fired;
    
    /// <summary>
    /// Gets the Id of the worker 
    /// </summary>
    public int Id { get => id; }
    
    /// <summary>
    /// Gets the reference to the <see cref="Database"/>.
    /// </summary>
    /// <remarks>
    /// Every insert, update, remove will execute on this database.
    /// </remarks>
    public Database Database { get => database; }
    public string Name { get => name; }
    public string Surname { get => surname; }
    public DateTime BirthDate { get => birthDate; }
    public Association Association
    {
        get
        {
            return Association.GetById(associationId, database);
        }
    }
    public bool Fired { get => fired; }

    
    /// <summary>
    /// Crates the worker wthout inserting it into the database.
    /// </summary>
    /// <remarks>
    /// The <see cref="Id"/> is set to -1 to indicate that this object is not in the database.
    /// To add it to the database use the <see cref="Insert()"/> method.
    /// </remarks>
    /// <param name="name">Name of the worker</param>
    /// <param name="surname">Surname of the worker</param>
    /// <param name="birthDate">That the worker was born</param>
    /// <param name="association">Association under which the worker is employed</param>
    /// <param name="fired">If true the worker is fired from the association</param>
    /// <param name="database">Database to use for insertions and stuff</param>
    public Worker(string name, string surname, DateTime birthDate, Association association, bool fired, Database database)
    {
         this.id = -1;
         this.database = database;
         this.name = name;
         this.surname = surname;
         this.birthDate = birthDate;
         this.associationId = association.Id;
         this.fired = fired;
    }

    /// <summary>
    /// Creates the worker that is mapped to an existing database record.
    /// </summary>
    /// <remarks>
    /// This constructor is meant for internal stuff.
    /// To get an worker from the database use the <see cref="GetById(int, Database)"/> or <see cref="GetByName(string, string, Database)"/> method instead.
    /// </remarks>
    /// <param name="id">Id of the database record</param>
    /// <param name="name">Name of the worker</param>
    /// <param name="surname">Surname of the worker</param>
    /// <param name="birthDate">That the worker was born</param>
    /// <param name="associationId">Id of the association under which the worker is employed</param>
    /// <param name="fired">If true the worker is fired from the association</param>
    /// <param name="database">Database to use for insertions and stuff</param>
    public Worker(int id, string name, string surname, DateTime birthDate, int associationId, bool fired, Database database)
    {
         this.id = id;
         this.database = database;
         this.name = name;
         this.surname = surname;
         this.birthDate = birthDate;
         this.associationId = associationId;
         this.fired = fired;
    }

    /// <summary>
    /// Inserts the worker into the database.
    /// </summary>
    /// <remarks>
    /// This method should not be executed twice.
    /// This method changes the <see cref="Id"/> property.
    /// </remarks>
    /// <exception cref="DatabaseException">Thrown if the worker was already inserted into the database</exception>
    /// <exception cref="SqlException">Thrown if anything bad happens with the database.<\exception>
    public void Insert()
    {
         if(id != - 1) throw new DatabaseException("Worker is already in database");
         
         using var command = new SqlCommand("INSERT INTO Workers (name, surname, birthDate, idAssociation, fired) VALUES (@name, @surname, @birthDate, @idAssociation, @fired)", database.Connection);
         command.Parameters.AddWithValue("@name", name);
         command.Parameters.AddWithValue("@surname", surname);
         command.Parameters.AddWithValue("@birthDate", birthDate);
         command.Parameters.AddWithValue("@idAssociation", associationId);
         command.Parameters.AddWithValue("@fired", fired);
         command.ExecuteNonQuery(); 
         
         // Get the id 
         using var command2 = new SqlCommand("SELECT TOP 1 idWorker FROM Workers ORDER BY idWorker DESC", database.Connection);
         var result = command2.ExecuteScalar();
         id = Convert.ToInt32(result); 
    }

    /// <summary>
    /// Removes the worker from the database.
    /// </summary>
    /// <remarks>
    /// This function should not be executed twice.
    /// This method changes the <see cref="Id"/> proerty to <c>-1</c>
    /// </remarks>
    /// <exception cref="DatabaseException">Thrown if the worker was not inserted into the database</exception>
    /// <exception cref="SqlException">Thrown if anything bad happens with the database.</exception>
    public void Remove()
    {
         if(id == -1) throw new DatabaseException("Worker not in database");
         
         using var command = new SqlCommand("DELETE FROM Workers WHERE idWorker = @idWorker", database.Connection);
         command.Parameters.AddWithValue("@idWorker", id);
         command.ExecuteNonQuery();
         id = -1;
    }

    /// <summary>
    /// Updates the worker record.
    /// </summary>
    /// <param name="name">Name of the worker</param>
    /// <param name="surname">Surname of the worker</param>
    /// <param name="birthDate">That the worker was born</param>
    /// <param name="association">Association under which the worker is employed</param>
    /// <param name="fired">If true the worker is fired from the association</param>
    /// <exception cref="DatabaseException">Thrown if the worker was not inserted into the database</exception>
    /// <exception cref="SqlException">Thrown if anything bad happens with the database.</exception>
    public void Update(string name, string surname, DateTime birthDate, Association association, bool fired)
    {
         if(id == -1) throw new DatabaseException("Worker not in database");
         
         using var command = new SqlCommand("UPDATE Workers SET name = @name, surname = @surname, birthDate = @birthDate, idAssociation = @idAssociation, fired = @fired WHERE idWorker = @idWorker", database.Connection);
         command.Parameters.AddWithValue("@name", name);
         command.Parameters.AddWithValue("@surname", surname);
         command.Parameters.AddWithValue("@birthDate", birthDate);
         command.Parameters.AddWithValue("@idAssociation", association.Id);
         command.Parameters.AddWithValue("@fired", fired);
         command.Parameters.AddWithValue("@idWorker", id);
         command.ExecuteNonQuery();
     
         this.name = name;
         this.surname = surname;
         this.birthDate = birthDate;
         this.associationId = association.Id;
         this.fired = fired;
    }

    /// <summary>
    /// Gets a worker from the database.
    /// </summary>
    /// <param name="id">Id of the worker</param>
    /// <param name="database">Database that contains the worker</param>
    /// <returns>Worker from the database</returns>
    public static Worker GetById(int id, Database database)
    {
         using var command = new SqlCommand("SELECT idWorker, name, surname, birthDate, idAssociation, fired FROM Workers WHERE idWorker = @id", database.Connection);
         command.Parameters.AddWithValue("@id", id);
         using var reader = command.ExecuteReader();
         if(reader.Read())
         {
             return new Worker(reader.GetInt32(0),
                     reader.GetString(1),
                     reader.GetString(2),
                     reader.GetDateTime(3),
                     reader.GetInt32(4),
                     reader.GetBoolean(5), 
                     database); 
         }
         else
         {
             throw new DatabaseException($"Field with id {id} not found"); 
         }
    }

    /// <summary>
    /// Gets a worker by its name.
    /// </summary> <remarks>
    /// This method gets only the last record from the database.
    /// If the database contains more than one worker only the last one is returned.
    /// </remarks>
    /// <param name="name">Name of the worker</param>
    /// <param name="surname">Surame of the worker</param>
    /// <param name="database">Database that contains the worker</param>
    /// <returns>Worker from the database</returns>
    public static Worker GetByName(string name, string surname, Database database)
    {
         using var command = new SqlCommand("SELECT idWorker, name, surname, birthDate, idAssociation, fired FROM Workers WHERE name = @name AND surname = @surname", database.Connection);
         command.Parameters.AddWithValue("@name", name);
         command.Parameters.AddWithValue("@surname", surname);
         using var reader = command.ExecuteReader();
         if(reader.Read())
         {
             return new Worker(reader.GetInt32(0),
                     reader.GetString(1),
                     reader.GetString(2),
                     reader.GetDateTime(3),
                     reader.GetInt32(4),
                     reader.GetBoolean(5), 
                     database); 
         }
         else
         {
             throw new DatabaseException($"Field with name {name} {surname} not found"); 
         }
    }

    /// <summary>
    /// Gets all worker from the database.
    /// </summary>
    /// <param name="database">Database with the workers</param>
    /// <returns>Workers from the database</returns>
    public static Worker[] GetAll(Database database)
    {
        List<Worker> list = new List<Worker>();
        using var command = new SqlCommand("SELECT idWorker, name, surname, birthDate, idAssociation, fired FROM Workers ORDER BY idWorker DESC",
                database.Connection);
        using var reader = command.ExecuteReader();
        
        while(reader.Read())
        {
            list.Add(new Worker(reader.GetInt32(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetDateTime(3),
                        reader.GetInt32(4),
                        reader.GetBoolean(5),
                        database)); 
        }
        
        return list.ToArray();
    }

    /// <summary>
    /// Gets max workers from the database.
    /// </summary>
    /// <param name="database">Datbase with the worker</param>
    /// <param name="max">Maximum number of columns that will be returned</param>
    /// <returns>Array with the workers</returns>
    public static Worker[] GetAll(Database database, int max)
    {
        List<Worker> list = new List<Worker>();
        using var command = new SqlCommand($"SELECT TOP {max} idWorker, name, surname, birthDate, idAssociation, fired FROM Workers ORDER BY idWorker DESC",
                database.Connection);
        using var reader = command.ExecuteReader();
        
        while(reader.Read())
        {
            list.Add(new Worker(reader.GetInt32(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetDateTime(3),
                        reader.GetInt32(4),
                        reader.GetBoolean(5),
                        database));
        }
        
        return list.ToArray();
    }

    public override string ToString()
    {
        return $" - name: {name}\n - surname: {surname}\n - birthDate: {birthDate.ToShortDateString()}\n - Association: \n{Utility.AddTabs(Association.ToString(), 1)}";
    }
}

