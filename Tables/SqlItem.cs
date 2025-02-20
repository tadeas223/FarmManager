public interface SqlItem<T> where T : SqlItem<T>
{
    int Id { get; }
    Database Database { get; }
    void Insert();
    void Remove();
}
