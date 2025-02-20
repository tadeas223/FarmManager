public class AddFieldCommand : IDBCommand
{
    public string Execute(string[] args, Database database)
    {
        Console.Write("name: ");
        string name = Console.ReadLine()!;

        Console.Write("size: ");
        string sizeStr = Console.ReadLine()!;
        
        float size;
        try
        {
            size = float.Parse(sizeStr);
        }
        catch
        {
            return "AddFieldCommand: Not a number";
        }

        Console.Write("association name: ");
        string associationName = Console.ReadLine()!;
        
        Association association;
        try
        {
            association = Association.GetByName(associationName, database);
        }
        catch
        {
            return "AddFieldCommand: Association not found";
        }

        Field field = new Field(name, size, association, database);
        field.Insert();
        
        return "Success";
    }
}
