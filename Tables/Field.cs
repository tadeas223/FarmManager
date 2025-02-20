using Microsoft.Data.SqlClient;

public class Field : SqlItem<Field>
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
        this.name = name;
        this.size = size;
        this.association = association;
        this.database = database;
        this.id = -1; 
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

}
