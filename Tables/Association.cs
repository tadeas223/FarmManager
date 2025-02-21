using Microsoft.Data.SqlClient;

/// <summary>
/// Represents a association stored in a database.
/// </summary>
public class Association : SqlItem
{
    private int id;
    private Database database;
    private string name;
    
    /// <summary>
    /// Gets the Id of the association in SQL database.
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
   
    /// <summary>
    /// Crates the association without inserting it into the database.
    /// </summary>
    /// <remarks>
    /// The <see cref="Id"/> is set to -1 to indicate that this object is not in the database.
    /// To add it to the database use the <see cref="Insert()"/> method.
    /// </remarks>
    /// <param name="name">Name of the association</param>
    /// <param name="database">Database to use for insertions and stuff</param>
    public Association(string name, Database database)
    {
        id = -1; 
        this.database = database;
        this.name = name;
    }
    
    /// <summary>
    /// Creates the association that is mapped to an existing database record.
    /// </summary>
    /// <remarks>
    /// This constructor is meant for internal stuff.
    /// To get an association from the database use the <see cref="GetById(int, Database)"/> or <see cref="GetByName(string, Database)"/> method instead.
    /// </remarks>
    /// <param name="id">Id of the database record</param>
    /// <param name="name">Name of the association</param>
    /// <param name="database">Database to use for insertions and stuff</param>
    public Association(int id, string name, Database database)
    {
        this.id = id;
        this.database = database;
        this.name = name;
    }
    
    /// <summary>
    /// Inserts the association into the database.
    /// </summary>
    /// <remarks>
    /// This method should not be executed twice.
    /// This method changes the <see cref="Id"/> property.
    /// </remarks>
    /// <exception cref="DatabaseException">Thrown if the association was already inserted into the database</exception>
    /// <exception cref="SqlException">Thrown if anything bad happens with the database.<\exception>
    public void Insert() 
    {
        if(id != - 1) throw new DatabaseException("Association is already in database");

        using var command = new SqlCommand("INSERT INTO Associations (name) VALUES (@name)", database.Connection);
        command.Parameters.AddWithValue("@name", Name);
        command.ExecuteNonQuery();
        
        // Get the id 
        using var command2 = new SqlCommand("SELECT TOP 1 idAssociation FROM Associations ORDER BY idAssociation DESC", database.Connection);
        var result = command2.ExecuteScalar();
        id = Convert.ToInt32(result); 
    }
    
    /// <summary>
    /// Removes the association from the database.
    /// </summary>
    /// <remarks>
    /// This function should not be executed twice.
    /// This method changes the <see cref="Id"/> proerty to <c>-1</c>
    /// </remarks>
    /// <exception cref="DatabaseException">Thrown if the association was not inserted into the database</exception>
    /// <exception cref="SqlException">Thrown if anything bad happens with the database.</exception>
    public void Remove()
    {
        if(id == -1) throw new DatabaseException("Association not in database");
        
        using var command = new SqlCommand("DELETE FROM Associations WHERE idAssociation = @idAssociation", database.Connection);
        command.Parameters.AddWithValue("@idAssociation", id);
        command.ExecuteNonQuery(); 
        id = -1;
    }
    
    /// <summary>
    /// Updates the association record.
    /// </summary>
    /// <param name="name">New name</param>
    /// <exception cref="DatabaseException">Thrown if the association was not inserted into the database</exception>
    /// <exception cref="SqlException">Thrown if anything bad happens with the database.</exception>
    public void Update(string name)
    {
        if(id == -1) throw new DatabaseException("Association not in database");
        
        using var command = new SqlCommand("UPDATE Associations SET name = @name WHERE idAssociation = @idAssociation", database.Connection);
        command.Parameters.AddWithValue("@name", name);
        command.Parameters.AddWithValue("@idAssociation", id);
        command.ExecuteNonQuery(); 
        
        this.name = name;
    }

    /// <summary>
    /// Gets an association from the database.
    /// </summary>
    /// <param name="id">Id of the association</param>
    /// <param name="database">Database that contains the association</param>
    /// <returns>Association recieved from the database</returns>
    public static Association GetById(int id, Database database)
    {
        using var command = new SqlCommand("SELECT idAssociation, name FROM Associations WHERE idAssociation = @id", database.Connection);
        command.Parameters.AddWithValue("@id", id);
        using var reader = command.ExecuteReader(); 
        if(reader.Read())
        {
            return new Association(reader.GetInt32(0), reader.GetString(1), database);
        }
        throw new DatabaseException($"Association with id {id} not found"); 
    }
    
    /// <summary>
    /// Gets an association by its name.
    /// </summary>
    /// <remarks>
    /// This method gets only the last record from the database.
    /// If the database contains more than one association only the last one is returned.
    /// </remarks>
    /// <param name="database">Database that contains the association</param>
    /// <returns>Association recieved from the database</returns>
    public static Association GetByName(string name, Database database)
    {
        using var command = new SqlCommand("SELECT idAssociation, name FROM Associations WHERE name = @name", database.Connection);
        command.Parameters.AddWithValue("@name", name);
        using var reader = command.ExecuteReader(); 
        if(reader.Read())
        {
            return new Association(reader.GetInt32(0), reader.GetString(1), database);
        }
        else
        {
            throw new DatabaseException($"Association with name {name} not found"); 
        }
    }
    
    /// <summary>
    /// Gets all associations from the database.
    /// </summary>
    /// <param name="database">Database with the associations</param>
    /// <returns>Array with associations</returns>
    public static Association[] GetAll(Database database)
    {
        List<Association> list = new List<Association>();
        using var command = new SqlCommand("SELECT idAssociation, name FROM Associations ORDER BY idAssociation DESC", database.Connection);
        using var reader = command.ExecuteReader();
        
        while(reader.Read())
        {
            
            list.Add(new Association(reader.GetInt32(0), reader.GetString(1), database));
        }
        
        return list.ToArray();
    }
    
    /// <summary>
    /// Gets a custom number of associations from the database.
    /// </summary>
    /// <param name="database">Database with the associations</param>
    /// <param name="max">Maximum number of columns that will be returned</param>
    /// <returns>Associations from the database</returns>
    public static Association[] GetAll(Database database, int max)
    {
        List<Association> list = new List<Association>();
        using var command = new SqlCommand($"SELECT TOP {max} idAssociation, name FROM Associations ORDER BY idAssociation DESC", database.Connection);
        using var reader = command.ExecuteReader();
        
        while(reader.Read())
        {
            
            list.Add(new Association(reader.GetInt32(0), reader.GetString(1), database));
        }
        
        return list.ToArray();
    }

    public override string ToString()
    {
        return $" - name: {name}";
    }
}
