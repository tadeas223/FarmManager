public class RemoveFieldCommand : IDBCommand
{
    public string Execute(string[] args, Database database)
    {
        Console.Write("name: ");
        string name = Console.ReadLine()!;

        Field field;
        try
        {
            field = Field.GetByName(name, database);
        }
        catch
        {
            return "RemoveFieldCommand: Field not found"; 
        }
        
        try
        {
            field.Remove();
        }
        catch
        {
            return "RemoveFieldCommand: Failed to remove Field";
        }
        return "Success";
    }
}
