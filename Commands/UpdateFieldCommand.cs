using System.Text.RegularExpressions;
public class UpdateFieldCommand : IDBCommand
{
    public string Execute(string[] args, Database database)
    {
        Console.Write("name: ");
        string fieldName = Console.ReadLine()!;

        Field field;
        try
        {
            field = Field.GetByName(fieldName, database);
        }
        catch
        {
            return "UpdateFieldCommand: Field not found";
        }
        
        Association association = field.Association;
        string name = field.Name;
        float size = field.Size;

        Console.WriteLine("Specify what to update, devide with ,");
        Console.Write("update: ");
        
        string[] split = Regex.Split(Console.ReadLine()!.ToLower(), "[,][ ]?");
        
        foreach(string s in split)
        {
            if(s.Equals("name"))
            {
                Console.Write("new name: ");
                name = Console.ReadLine()!;
            }
            if(s.Equals("size"))
            {
                Console.Write("new size: ");
                try
                {
                    size = float.Parse(Console.ReadLine()!);
                }
                catch
                {
                    return "UpdateCropCommand: Not a float";
                }
            }
            if(s.Equals("association"))
            {
                Console.Write("new association name: ");
                string associationName = Console.ReadLine()!;

                try
                {
                    association = Association.GetByName(associationName, database);
                }
                catch
                {
                    return "UpdateFieldCommand: Association not found";
                }
            }
        }

        try
        {
            field.Update(name, size, association);
        }
        catch
        {
            return "UpdateFieldCommand: Failed to update Field";
        }

        return "Success";
    }
}
