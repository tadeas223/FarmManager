using Microsoft.Data.SqlClient;

/// <summary>
/// Represents a sowing record stored in a database.
/// </summary>
public class SowingRecord : SqlItem
{

    private int id;
    private Database database;
    private int cropId;
    private int fieldId;
    private int workerId;
    private DateTime date;
    

    /// <summary>
    /// Gets the Id of the sowing record
    /// </summary>
    public int Id { get => id; }

    /// <summary>
    /// Gets the reference to the <see cref="Database"/>.
    /// </summary>
    /// <remarks>
    /// Every insert, update, remove will execute on this database.
    /// </remarks>
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

    /// <summary>
    /// Crates the sowing record wthout inserting it into the database.
    /// </summary>
    /// <remarks>
    /// The <see cref="Id"/> is set to -1 to indicate that this object is not in the database.
    /// To add it to the database use the <see cref="Insert()"/> method.
    /// </remarks>
    /// <param name="crop">Crop that was sowed</param>
    /// <param name="field">Field that was sowded</param>
    /// <param name="worker">Worker that did the sowing</param>
    /// <param name="date">Date of the sowing</param>
    /// <param name="database">Database to use for insertions and stuff</param>
    public SowingRecord(Crop crop, Field field, Worker worker, DateTime date, Database database)
    {
        this.id = -1;
        this.database = database;
        this.cropId = crop.Id;
        this.fieldId = field.Id;
        this.workerId = worker.Id;
        this.date = date;
    }

    /// <summary>
    /// Creates the sowing record that is mapped to an existing database record.
    /// </summary>
    /// <remarks>
    /// This constructor is meant for internal stuff.
    /// To get an sowing record from the database use the <see cref="GetById(int, Database)"/> or <see cref="GetByFieldDate(Field, DateTime, Database)"/> method instead.
    /// </remarks>
    /// <param name="id">Id of the database record</param>
    /// <param name="crop">Crop that was sowed</param>
    /// <param name="field">Field that was sowded</param>
    /// <param name="worker">Worker that did the sowing</param>
    /// <param name="date">Date of the sowing</param>
    /// <param name="database">Database to use for insertions and stuff</param>
    public SowingRecord(int id, int cropId, int fieldId, int workerId, DateTime date, Database database)
    {
        this.id = id;
        this.database = database;
        this.cropId = cropId;
        this.fieldId = fieldId;
        this.workerId = workerId;
        this.date = date;
    }

    /// <summary>
    /// Inserts the sowing record into the database.
    /// </summary>
    /// <remarks>
    /// This method should not be executed twice.
    /// This method changes the <see cref="Id"/> property.
    /// </remarks>
    /// <exception cref="DatabaseException">Thrown if the sowing record was already inserted into the database</exception>
    /// <exception cref="SqlException">Thrown if anything bad happens with the database.<\exception>
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

    /// <summary>
    /// Removes the sowing record from the database.
    /// </summary>
    /// <remarks>
    /// This function should not be executed twice.
    /// This method changes the <see cref="Id"/> proerty to <c>-1</c>
    /// </remarks>
    /// <exception cref="DatabaseException">Thrown if the sowing record was not inserted into the database</exception>
    /// <exception cref="SqlException">Thrown if anything bad happens with the database.</exception>
    public void Remove()
    {
        if(id == -1) throw new DatabaseException("SowingRecord not in database");
        
        using var command = new SqlCommand("DELETE FROM SowingRecords WHERE idSowingRecord = @idSowingRecord", database.Connection);
        command.Parameters.AddWithValue("@idSowingRecord", id);
        command.ExecuteNonQuery();
        id = -1;
    }
    
    /// <summary>
    /// Updates the sowing record.
    /// </summary>
    /// <param name="crop">Crop that was sowed</param>
    /// <param name="field">Field that was sowded</param>
    /// <param name="worker">Worker that did the sowing</param>
    /// <param name="date">Date of the sowing</param>
    /// <exception cref="DatabaseException">Thrown if the sowing record was not inserted into the database</exception>
    /// <exception cref="SqlException">Thrown if anything bad happens with the database.</exception>
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

    /// <summary>
    /// Gets a sowing record from the database.
    /// </summary>
    /// <param name="id">Id of the sowing record</param>
    /// <param name="database">Database that contains the sowing record</param>
    /// <returns>Sowing record from the database</returns>
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

    /// <summary>
    /// Gets a sowing record by the field and a date.
    /// </summary>
    /// <remarks>
    /// This method gets only the last record from the database.
    /// If the database contains more than one sowing record only the last one is returned.
    /// </remarks>
    /// <param name="field">Field idk</param>
    /// <param name="date">Date of the sowing</param>
    /// <param name="database">Database that contains the sowing record</param>
    /// <returns>Sowing record from the database</returns>
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

    /// <summary>
    /// Gets all sowing records from the database.
    /// </summary>
    /// <param name="database">Database with the records</param>
    /// <returns>Sowing records from the database</returns>
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

    /// <summary>
    /// Gets a custom number of sowing records from the database.
    /// </summary>
    /// <param name="database">Database with the sowing record</param>
    /// <param name="max">Maximum number of columns that will be returned</param>
    /// <returns>Array with the sowing records</returns>
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
