using Microsoft.Data.SqlClient;

public class Field : SqlItem
{
    private int id;
    private Database database;
    private string name;
    private float size;
    private Association association;


    public int Id { get => id;}
    public Database Database { get => database; }
    public string Name { get => name; }
    public float Size { get => size; }
    public Association Associaiton { get => association; }

    public Field(string name, float size, Association association, Database database)
    {
        this.id = -1; 
        this.database = database;
        this.name = name;
        this.size = size;
        this.association = association;
    }

    public Field(int id, string name, float size, Association association, Database database)
    {
        this.id = id; 
        this.database = database;
        this.name = name;
        this.size = size;
        this.association = association;
    }

    public void Insert()
    {
        if(id != - 1) throw new DatabaseException("Field is already in database");
        
        using var command = new SqlCommand("INSERT INTO Fields (name, size, idAssociation) VALUES (@name, @size, @idAssociation)", database.Connection);
        command.Parameters.AddWithValue("@name", name);
        command.Parameters.AddWithValue("@size", size);
        command.Parameters.AddWithValue("@idAssociation", association.Id);
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

    public static Field GetById(int id, Database database)
    {
        using var command = new SqlCommand("SELECT idField, name, size, idAssociation FROM Fields WHERE idField = @id",database.Connection);
        command.Parameters.AddWithValue("@id", id);
        var reader = command.ExecuteReader();

        if(reader.Read())
        {
            return new Field(reader.GetInt32(0), reader.GetString(1), reader.GetFloat(2), Association.GetById(reader.GetInt32(3), database), database);
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
        var reader = command.ExecuteReader();

        if(reader.Read())
        {
            return new Field(reader.GetInt32(0), reader.GetString(1), reader.GetFloat(2), Association.GetById(reader.GetInt32(3), database), database);
        }
        else
        {
            throw new DatabaseException($"Field with name {name} not found"); 
        }
    }

    public static Field[] GetAll(Database database)
    {
        List<Field> list = new List<Field>();
        using var command = new SqlCommand("SELECT idField, name, size, idAssociation FROM Fields ORDER BY idField DESC",database.Connection);
        using var reader = command.ExecuteReader();
        
        while(reader.Read())
        {
            
            list.Add(new Field(reader.GetInt32(0), reader.GetString(1), reader.GetFloat(2), Association.GetById(reader.GetInt32(3), database), database));
        }
        
        return list.ToArray();
    }

    public static Field[] GetAll(Database database, int max)
    {
        List<Field> list = new List<Field>();
        using var command = new SqlCommand($"SELECT TOP {max} idField, name, size, idAssociation FROM Fields ORDER BY idField DESC",database.Connection);
        using var reader = command.ExecuteReader();
        
        while(reader.Read())
        {
            
            list.Add(new Field(reader.GetInt32(0), reader.GetString(1), reader.GetFloat(2), Association.GetById(reader.GetInt32(3), database), database));
        }
        
        return list.ToArray();
    }
}
