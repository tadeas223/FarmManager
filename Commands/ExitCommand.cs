public class ExitCommand : IDBCommand
{
    public string Execute(string[] args, Database database)
    {
        database.Dispose();
        Console.WriteLine("Bye :)");
        Environment.Exit(0);
        return "This will never return";
    }
}
