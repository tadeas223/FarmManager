using Microsoft.Data.SqlClient;

public class SowingRecord : SqlItem
{

    private int id;
    private Database database;
    private int cropId;
    private int fieldId;
    private int workerId;
    private DateTime date;

    public int Id { get => id; }
    public Database Database { get => database; }

    public Field Field
    {
        get
        {
            return Field.GetById(fieldId, database);
        }
    }
    public Worker Worker
    {
        get
        {
            return Worker.GetById(workerId, database);
        }
    }
    public Crop Crop
    {
        get
        {
            return Crop.GetById(cropId, database);
        }
    }

    public DateTime Date { get => date; }

    public SowingRecord(Crop crop, Field field, Worker worker, DateTime date, Database database)
    {
        this.id = -1;
        this.database = database;
        this.cropId = crop.Id;
        this.fieldId = field.Id;
        this.workerId = worker.Id;
        this.date = date;
    }

    public SowingRecord(int id, int cropId, int fieldId, int workerId, DateTime date, Database database)
    {
        this.id = id;
        this.database = database;
        this.cropId = cropId;
        this.fieldId = fieldId;
        this.workerId = workerId;
        this.date = date;
    }

    public void Insert()
    {
        if(id != - 1) throw new DatabaseException("SowingRecord is already in database");
        
        using var command = new SqlCommand("INSERT INTO SowingRecords (date, idCrop, idWorker, idField) VALUES (@date, @idCrop, @idWorker, @idField)", database.Connection);
        command.Parameters.AddWithValue("@date", date);
        command.Parameters.AddWithValue("@idCrop", cropId);
        command.Parameters.AddWithValue("@idWorker", workerId);
        command.Parameters.AddWithValue("@idField", fieldId);
        command.ExecuteNonQuery(); 
        
        // Get the id 
        using var command2 = new SqlCommand("SELECT TOP 1 idSowingRecord FROM SowingRecords ORDER BY idSowingRecord DESC", database.Connection);
        var result = command2.ExecuteScalar();
        id = Convert.ToInt32(result); 
    }

    public void Remove()
    {
        if(id == -1) throw new DatabaseException("SowingRecord not in database");
        
        using var command = new SqlCommand("DELETE FROM SowingRecords WHERE idSowingRecord = @idSowingRecord", database.Connection);
        command.Parameters.AddWithValue("@idSowingRecord", id);
        command.ExecuteNonQuery();
        id = -1;
    }
    
    public void Update(Crop crop, Field field, Worker worker, DateTime date)
    {
        if(id == -1) throw new DatabaseException("SowingRecord not in database");
        
        using var command = new SqlCommand("UPDATE SowingRecords SET idCrop = @idCrop, idField = @idField, idWorker = @idWorker, date = @date WHERE idSowingRecord = @idSowingRecord", database.Connection);
        command.Parameters.AddWithValue("@idCrop", crop.Id);
        command.Parameters.AddWithValue("@idField", field.Id);
        command.Parameters.AddWithValue("@idWorker", worker.Id);
        command.Parameters.AddWithValue("@date", date);
        command.Parameters.AddWithValue("@idSowingRecord", id);
        command.ExecuteNonQuery();
    
        this.cropId = crop.Id;
        this.fieldId = field.Id;
        this.workerId = worker.Id;
        this.date = date;
    }

    public static SowingRecord GetById(int id, Database database)
    {
        using var command = new SqlCommand("SELECT idSowingRecord, idCrop, idField, idWorker, date FROM SowingRecords WHERE idSowingRecord = @id", database.Connection);
        command.Parameters.AddWithValue("@id", id);
        using var reader = command.ExecuteReader();
        if(reader.Read())
        {
            return new SowingRecord(reader.GetInt32(0),
                    reader.GetInt32(1),
                    reader.GetInt32(2),
                    reader.GetInt32(3),
                    reader.GetDateTime(4),
                    database);
        }
        else
        {
            throw new DatabaseException($"SowingRecord with id {id} not found"); 
        }
    }

    public static SowingRecord GetByFieldDate(Field field, DateTime date, Database database)
    {
        using var command = new SqlCommand("SELECT idSowingRecord, idCrop, idField, idWorker, date FROM SowingRecords WHERE idField = @idField AND date = @date", database.Connection);
        command.Parameters.AddWithValue("@idField", field.Id);
        command.Parameters.AddWithValue("@date", date);
        using var reader = command.ExecuteReader();
        if(reader.Read())
        {
            return new SowingRecord(reader.GetInt32(0),
                    reader.GetInt32(1),
                    reader.GetInt32(2),
                    reader.GetInt32(3),
                    reader.GetDateTime(4),
                    database);
        }
        else
        {
            throw new DatabaseException("SowingRecord not found"); 
        }
    }

    public static SowingRecord[] GetAll(Database database)
    {
        List<SowingRecord> list = new List<SowingRecord>();
        using var command = new SqlCommand("SELECT idSowingRecord, idCrop, idField, idWorker, date FROM SowingRecords ORDER BY idSowingRecord DESC", database.Connection);
        using var reader = command.ExecuteReader();
        
        while(reader.Read())
        {
            list.Add(new SowingRecord(reader.GetInt32(0),
                    reader.GetInt32(1),
                    reader.GetInt32(2),
                    reader.GetInt32(3),
                    reader.GetDateTime(4),
                    database));
        }
        
        return list.ToArray();
    }

    public static SowingRecord[] GetAll(Database database, int max)
    {
        List<SowingRecord> list = new List<SowingRecord>();
        using var command = new SqlCommand($"SELECT TOP {max} idSowingRecord, idCrop, idField, idWorker, date FROM SowingRecords ORDER BY idSowingRecord DESC", database.Connection);
        using var reader = command.ExecuteReader();
        
        while(reader.Read())
        {
            
            list.Add(new SowingRecord(reader.GetInt32(0),
                    reader.GetInt32(1),
                    reader.GetInt32(2),
                    reader.GetInt32(3),
                    reader.GetDateTime(4),
                    database));
        }
        
        return list.ToArray();
    }

    public override string ToString()
    {
        return $" - Crop: \n{Utility.AddTabs(Crop.ToString(), 1)}\n - Field: \n{Utility.AddTabs(Field.ToString(), 1)}\n - Worker: {Utility.AddTabs(Worker.ToString(), 1)}\n - date: {date.ToShortDateString()}";
    }
}
