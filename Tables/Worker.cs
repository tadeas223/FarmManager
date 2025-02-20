using Microsoft.Data.SqlClient;

public class Worker : SqlItem<Worker> 
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

}

