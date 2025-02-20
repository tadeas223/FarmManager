public interface SqlItem
{
    int Id { get; }
    Database Database { get; }
    void Insert();
    void Remove();
}
