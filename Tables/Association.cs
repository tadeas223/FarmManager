using Microsoft.Data.SqlClient;

public class Association : SqlItem
{
    private int id;
    private Database database;
    private string name;

    public int Id { get => id; }
    public Database Database { get => database; }
    public string Name { get => name; }
    
    public Association(string name, Database database)
    {
        id = -1; 
        this.database = database;
        this.name = name;
    }

    public Association(int id, string name, Database database)
    {
        this.id = id;
        this.database = database;
        this.name = name;
    }

    public void Insert() 
    {
        if(id != - 1) throw new DatabaseException("Association is already in database");

        using var command = new SqlCommand("INSERT INTO Associations (name) VALUES (@name)", database.Connection);
        command.Parameters.AddWithValue("@name", Name);
        command.ExecuteNonQuery();
        
        // Get the id 
        using var command2 = new SqlCommand("SELECT TOP 1 idAssociation FROM Associations ORDER BY idAssociation DESC");
        var result = command2.ExecuteScalar();
        id = Convert.ToInt32(result); 
    }
    
    public void Remove()
    {
        if(id == -1) throw new DatabaseException("Association not in database");
        
        using var command = new SqlCommand("DELETE FROM Associations WHERE idAssociation = @idAssociation", database.Connection);
        command.Parameters.AddWithValue("@idAssociation", id);
        command.ExecuteNonQuery(); 
        id = -1;
    }

    public static Association GetById(int id, Database database)
    {
        using var command = new SqlCommand("SELECT idAssociation, name FROM Associations WHERE idAssociation = @id", database.Connection);
        command.Parameters.AddWithValue("@id", id);
        var reader = command.ExecuteReader(); 
        if(reader.Read())
        {
            return new Association(reader.GetInt32(0), reader.GetString(1), database);
        }
        else
        {
            throw new DatabaseException($"Association with id {id} not found"); 
        }
    }

}
