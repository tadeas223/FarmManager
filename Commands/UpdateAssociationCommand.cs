public class UpdateAssociationCommand : IDBCommand
{
    public string Execute(string[] args, Database database)
    {
        Console.Write("name: ");
        string name = Console.ReadLine()!;
        
        Association association;
        try
        {
            association = Association.GetByName(name, database);
        }
        catch
        {
            return "UpdateAssociationCommand: Association not found";
        }

        Console.Write("new name: ");
        string newName = Console.ReadLine()!;
        
        try
        {
            association.Update(newName);
        }
        catch
        {
            return "UpdateAssociationCommand: Failed to update Association";
        }

        return "Success";

    }
}
