using Microsoft.Data.SqlClient;

/// <summary>
/// Represents a crop stored in a database.
/// </summary>
public class Crop : SqlItem
{
    private int id;
    private Database database;
    private string name;
    private float price;
    
    /// <summary>
    /// Gets the Id of the crop
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
    public float Price { get => price; }
    
    /// <summary>
    /// Crates the crop wthout inserting it into the database.
    /// </summary>
    /// <remarks>
    /// The <see cref="Id"/> is set to -1 to indicate that this object is not in the database.
    /// To add it to the database use the <see cref="Insert"/> method.
    /// </remarks>
    /// <param name="name">Name of the crop</param>
    /// <param name="price">Price of the crop</param>
    /// <param name="database">Database to use for insertions and stuff</param>
    public Crop(string name, float price, Database database) 
    {
        this.id = -1;
        this.database = database;
        this.name = name;
        this.price = price;
    }

    /// <summary>
    /// Creates the crop that is mapped to an existing database record.
    /// </summary>
    /// <remarks>
    /// This constructor is meant for internal stuff.
    /// To get an crop from the database use the <see cref="GetById(int, Database)"/> or <see cref="GetByName(string, Database)"/> method instead.
    /// </remarks>
    /// <param name="id">Id of the database record</param>
    /// <param name="name">Name of the crop</param>
    /// <param name="price">Price of the crop</param>
    /// <param name="database">Database to use for insertions and stuff</param>
    public Crop(int id, string name, float price, Database database)
    {
        
        this.id = id;
        this.database = database;
        this.name = name;
        this.price = price;
    }

    /// <summary>
    /// Inserts the crop into the database.
    /// </summary>
    /// <remarks>
    /// This method should not be executed twice.
    /// This method changes the <see cref="Id"/> property.
    /// </remarks>
    /// <exception cref="DatabaseException">Thrown if the crop was already inserted into the database</exception>
    /// <exception cref="SqlException">Thrown if anything bad happens with the database.<\exception>
    public void Insert() 
    {
        if(id != - 1) throw new DatabaseException("Crop is already in database");

        using var command = new SqlCommand("INSERT INTO Crops (name, price) VALUES (@name, @price)", database.Connection);
        command.Parameters.AddWithValue("@name", name);
        command.Parameters.AddWithValue("@price", price);
        command.ExecuteNonQuery(); 
        
        // Get the id 
        using var command2 = new SqlCommand("SELECT TOP 1 idCrop FROM Crops ORDER BY idCrop DESC", database.Connection);
        var result = command2.ExecuteScalar();
        id = Convert.ToInt32(result);

    }

    /// <summary>
    /// Removes the crop from the database.
    /// </summary>
    /// <remarks>
    /// This function should not be executed twice.
    /// This method changes the <see cref="Id"/> proerty to <c>-1</c>
    /// </remarks>
    /// <exception cref="DatabaseException">Thrown if the crop was not inserted into the database</exception>
    /// <exception cref="SqlException">Thrown if anything bad happens with the database.</exception>
    public void Remove() 
    {
        if(id == -1) throw new DatabaseException("Crop not in database");
        
        using var command = new SqlCommand("DELETE FROM Crops WHERE idCrop = @idCrop", database.Connection);
        command.Parameters.AddWithValue("@idCrop", id);
        command.ExecuteNonQuery();
        id = -1;
    }

    /// <summary>
    /// Updates the crop record.
    /// </summary>
    /// <param name="name">New name</param>
    /// <param name="price">New price</param>
    /// <exception cref="DatabaseException">Thrown if the crop was not inserted into the database</exception>
    /// <exception cref="SqlException">Thrown if anything bad happens with the database.</exception>
    public void Update(string name, float price)
    {
        if(id == -1) throw new DatabaseException("Crop not in database");
        
        using var command = new SqlCommand("UPDATE Crops SET name = @name, price = @price WHERE idCrop = @idCrop", database.Connection);
        command.Parameters.AddWithValue("@name", name);
        command.Parameters.AddWithValue("@price", price);
        command.Parameters.AddWithValue("@idCrop", id);
        command.ExecuteNonQuery();
        
        this.name = name;
        this.price = price;
    }
    
    /// <summary>
    /// Gets a crop from the database.
    /// </summary>
    /// <param name="id">Id of the crop</param>
    /// <param name="database">Database that contains the crop</param>
    /// <returns>Crop from the database</returns>
    public static Crop GetById(int id, Database database)
    {
        using var command = new SqlCommand("SELECT idCrop, name, price FROM Crops WHERE idCrop = @id", database.Connection);
        command.Parameters.AddWithValue("@id", id);
        using var reader = command.ExecuteReader();
        if(reader.Read())
        {
            return new Crop(reader.GetInt32(0), reader.GetString(1), Convert.ToSingle(reader.GetDouble(2)), database);
        }
        else
        {
            throw new DatabaseException($"Crop with id {id} not found"); 
        }
    }

    /// <summary>
    /// Gets a crop by its name.
    /// </summary>
    /// <remarks>
    /// This method gets only the last record from the database.
    /// If the database contains more than one crop only the last one is returned.
    /// </remarks>
    /// <param name="database">Database that contains the crop</param>
    /// <returns>Crop from the database</returns>
    public static Crop GetByName(string name, Database database)
    {
        using var command = new SqlCommand("SELECT idCrop, name, price FROM Crops WHERE name = @name", database.Connection);
        command.Parameters.AddWithValue("@name", name);
        using var reader = command.ExecuteReader();
        if(reader.Read())
        {
            return new Crop(reader.GetInt32(0), reader.GetString(1), Convert.ToSingle(reader.GetDouble(2)), database);
        }
        else
        {
            throw new DatabaseException($"Crop with name {name} not found"); 
        }
    }

    /// <summary>
    /// Gets all crops from the database.
    /// </summary>
    /// <param name="database">Database with the crops</param>
    /// <returns>Crops from the database</returns>
    public static Crop[] GetAll(Database database)
    {
        List<Crop> list = new List<Crop>();
        using var command = new SqlCommand("SELECT idCrop, name, price FROM Crops ORDER BY idCrop DESC", database.Connection);
        using var reader = command.ExecuteReader();
        
        while(reader.Read())
        {
            
            list.Add(new Crop(reader.GetInt32(0), reader.GetString(1), Convert.ToSingle(reader.GetDouble(2)), database));
        }
        
        return list.ToArray();
    }

    /// <summary>
    /// Gets a specified number of crops from the database.
    /// </summary>
    /// <param name="database">Database with the crops</param>
    /// <param name="max">Maximum number of columns that will be returned</param>
    /// <returns>Array with crops</returns>
    public static Crop[] GetAll(Database database, int max)
    {
        List<Crop> list = new List<Crop>();
        using var command = new SqlCommand($"SELECT TOP {max} idCrop, name, price FROM Crops ORDER BY idCrop DESC", database.Connection);
        using var reader = command.ExecuteReader();
        
        while(reader.Read())
        {
            
            list.Add(new Crop(reader.GetInt32(0), reader.GetString(1), Convert.ToSingle(reader.GetDouble(2)), database));
        }
        
        return list.ToArray();
    }

    public override string ToString()
    {
        return $" - name: {name}\n - price: {price}";
    }
}
