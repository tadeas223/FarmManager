using Microsoft.Data.SqlClient;

public class Field : SqlItem
{
    private int id;
    private Database database;
    private string name;
    private float size;
    private int associationId;


    public int Id { get => id;}
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

    public Field(string name, float size, Association association, Database database)
    {
        this.id = -1; 
        this.database = database;
        this.name = name;
        this.size = size;
        this.associationId = association.Id;
    }

    public Field(int id, string name, float size, int associationId, Database database)
    {
        this.id = id; 
        this.database = database;
        this.name = name;
        this.size = size;
        this.associationId = associationId;
    }

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
    
    public void Remove()
    {
        if(id == -1) throw new DatabaseException("Field not in database");
        
        using var command = new SqlCommand("DELETE FROM Fields WHERE idField = @idField", database.Connection);
        command.Parameters.AddWithValue("@idfield", id);
        command.ExecuteNonQuery();
        id = -1;
    }

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
