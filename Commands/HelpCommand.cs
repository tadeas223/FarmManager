public class HelpCommand : IDBCommand
{
    public string Execute(string[] args, Database database)
    {
        return "show [table] [max]  - Shows info from the table";
    }
}
