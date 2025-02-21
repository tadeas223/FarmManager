/// <summary>
/// Inteface for SQL items idk.
/// </summary>
public interface SqlItem
{
    /// <summary>
    /// Gets the items id.
    /// <\summary>
    int Id { get; }
    /// <summary>
    /// Reference to the database to do operations on.
    /// </summary>
    Database Database { get; }

    /// <summary>
    /// Inserts the into into the database.
    /// </summary>
    void Insert();
    /// <summary>
    /// Removes the into into the database.
    /// </summary>
    void Remove();
}
