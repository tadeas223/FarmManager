using Microsoft.Data.SqlClient;

/// <summary>
/// Represents a field stored in a database.
/// </summary>
public class Field : SqlItem
{
    private int id;
    private Database database;
    private string name;
    private float size;
    private int associationId;


    /// <summary>
    /// Gets the Id of the field
    /// </summary>
    public int Id { get => id;}
    
    /// <summary>
    /// Gets the reference to the <see cref="Database"/>.
    /// </summary>
    /// <remarks>
    /// Every insert, update, remove will execute on this database.
    /// </remarks>
    public Database Database { get => database; }
    public string Name { get => name; }
    public float Size { get => size; }
    public Association Association
    {
        get
        {
            return Association.GetById(associationId, database); 
        }
    }

    /// <summary>
    /// Crates the Field wthout inserting it into the database.
    /// </summary>
    /// <remarks>
    /// The <see cref="Id"/> is set to -1 to indicate that this object is not in the database.
    /// To add it to the database use the <see cref="Insert"/> method.
    /// </remarks>
    /// <param name="name">Name of the field</param>
    /// <param name="size">Size of the field</param>
    /// <param name="association">Association that owns the field</param>
    /// <param name="database">Database to use for insertions and stuff</param>
    public Field(string name, float size, Association association, Database database)
    {
        this.id = -1; 
        this.database = database;
        this.name = name;
        this.size = size;
        this.associationId = association.Id;
    }

    /// <summary>
    /// Creates the field that is mapped to an existing database record.
    /// </summary>
    /// <remarks>
    /// This constructor is meant for internal stuff.
    /// To get an field from the database use the <see cref="GetById(int, Database)"/> or <see cref="GetByName(string, Database)"/> method instead.
    /// </remarks>
    /// <param name="id">Id of the database record</param>
    /// <param name="name">Name of the field</param>
    /// <param name="size">Size of the field</param>
    /// <param name="associationId">Id of an association that owns the field</param>
    /// <param name="database">Database to use for insertions and stuff</param>
    public Field(int id, string name, float size, int associationId, Database database)
    {
        this.id = id; 
        this.database = database;
        this.name = name;
        this.size = size;
        this.associationId = associationId;
    }

    /// <summary>
    /// Inserts the field into the database.
    /// </summary>
    /// <remarks>
    /// This method should not be executed twice.
    /// This method changes the <see cref="Id"/> property.
    /// </remarks>
    /// <exception cref="DatabaseException">Thrown if the field was already inserted into the database</exception>
    /// <exception cref="SqlException">Thrown if anything bad happens with the database.<\exception>
    public void Insert()
    {
        if(id != - 1) throw new DatabaseException("Field is already in database");
        
        using var command = new SqlCommand("INSERT INTO Fields (name, size, idAssociation) VALUES (@name, @size, @idAssociation)", database.Connection);
        command.Parameters.AddWithValue("@name", name);
        command.Parameters.AddWithValue("@size", size);
        command.Parameters.AddWithValue("@idAssociation", associationId);
        command.ExecuteNonQuery(); 
   
        // Get the id 
        using var command2 = new SqlCommand("SELECT TOP 1 idField FROM Fields ORDER BY idField DESC", database.Connection);
        var result = command2.ExecuteScalar();
        id = Convert.ToInt32(result); 
    }
    
    /// <summary>
    /// Removes the field from the database.
    /// </summary>
    /// <remarks>
    /// This function should not be executed twice.
    /// This method changes the <see cref="Id"/> proerty to <c>-1</c>
    /// </remarks>
    /// <exception cref="DatabaseException">Thrown if the field was not inserted into the database</exception>
    /// <exception cref="SqlException">Thrown if anything bad happens with the database.</exception>
    public void Remove()
    {
        if(id == -1) throw new DatabaseException("Field not in database");
        
        using var command = new SqlCommand("DELETE FROM Fields WHERE idField = @idField", database.Connection);
        command.Parameters.AddWithValue("@idfield", id);
        command.ExecuteNonQuery();
        id = -1;
    }

    /// <summary>
    /// Updates the field record.
    /// </summary>
    /// <param name="name">New name</param>
    /// <param name="size">New size</param>
    /// <param name="association">New Association</param>
    /// <exception cref="DatabaseException">Thrown if the field was not inserted into the database</exception>
    /// <exception cref="SqlException">Thrown if anything bad happens with the database.</exception>
    public void Update(string name, float size, Association association)
    {
        if(id == -1) throw new DatabaseException("Field not in database");
        
        using var command = new SqlCommand("UPDATE Fields SET name = @name, size = @size, idAssociation = @idAssociation WHERE idField = @idField", database.Connection);
        command.Parameters.AddWithValue("@name", name);
        command.Parameters.AddWithValue("@size", size);
        command.Parameters.AddWithValue("@idAssociation", association.Id);
        command.Parameters.AddWithValue("@idfield", id);
        command.ExecuteNonQuery();
    
        this.name = name;
        this.size = size;
        this.associationId = association.Id;
    }

    /// <summary>
    /// Gets a field from the database.
    /// </summary>
    /// <param name="id">Id of the field</param>
    /// <param name="database">Database that contains the field</param>
    /// <returns>Field from the database</returns>
    public static Field GetById(int id, Database database)
    {
        using var command = new SqlCommand("SELECT idField, name, size, idAssociation FROM Fields WHERE idField = @id",database.Connection);
        command.Parameters.AddWithValue("@id", id);
        using var reader = command.ExecuteReader();

        if(reader.Read())
        {
            return new Field(reader.GetInt32(0), reader.GetString(1), Convert.ToSingle(reader.GetDouble(2)), reader.GetInt32(3), database);
        }
        else
        {
            throw new DatabaseException($"Field with id {id} not found"); 
        }
    }

    /// <summary>
    /// Gets a field by its name.
    /// </summary>
    /// <remarks>
    /// This method gets only the last record from the database.
    /// If the database contains more than one Field only the last one is returned.
    /// </remarks>
    /// <param name="name">Name of the field</param>
    /// <param name="database">Database that contains the field</param>
    /// <returns>Field from the database</returns>
    public static Field GetByName(string name, Database database)
    {
        using var command = new SqlCommand("SELECT idField, name, size, idAssociation FROM Fields WHERE name = @name",database.Connection);
        command.Parameters.AddWithValue("@name", name);
        using var reader = command.ExecuteReader();

        if(reader.Read())
        {
            return new Field(reader.GetInt32(0), reader.GetString(1), Convert.ToSingle(reader.GetDouble(2)), reader.GetInt32(3), database);
        }
        else
        {
            throw new DatabaseException($"Field with name {name} not found"); 
        }
    }

    /// <summary>
    /// Gets all fields from the database.
    /// </summary>
    /// <param name="database">Database with the fields</param>
    /// <returns>Fields from the database</returns>
    public static Field[] GetAll(Database database)
    {
        List<Field> list = new List<Field>();
        using var command = new SqlCommand("SELECT idField, name, size, idAssociation FROM Fields ORDER BY idField DESC", database.Connection);
        using var reader = command.ExecuteReader();
        
        while(reader.Read())
        {
            list.Add(new Field(reader.GetInt32(0), reader.GetString(1), Convert.ToSingle(reader.GetDouble(2)), reader.GetInt32(3), database));
        }
        
        return list.ToArray();
    }

    /// <summary>
    /// Gets a custom number of fields from the database.
    /// </summary>
    /// <param name="database">Database with the field</param>
    /// <param name="max">Maximum number of columns that will be returned</param>
    /// <returns>Array with fields</returns>
    public static Field[] GetAll(Database database, int max)
    {
        List<Field> list = new List<Field>();
        using var command = new SqlCommand($"SELECT TOP {max} idField, name, size, idAssociation FROM Fields ORDER BY idField DESC", database.Connection);
        using var reader = command.ExecuteReader();
        
        while(reader.Read())
        {
            list.Add(new Field(reader.GetInt32(0), reader.GetString(1), Convert.ToSingle(reader.GetDouble(2)), reader.GetInt32(3), database));
        }
        
        return list.ToArray();
    }

    public override string ToString()
    {
        return $" - name: {name}\n - size: {size}\n - Association: \n{Utility.AddTabs(Association.ToString(), 1)}";
    }
}
