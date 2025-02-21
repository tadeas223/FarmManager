public class RemoveAssociationCommand : IDBCommand
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
            return "RemoveAssociationCommand: Association not found";
        }
    
        try
        {
            association.Remove();
        }
        catch (Exception e)
        {
            throw e;
            return "RemoveAssociationCommand: Failed to remove Association\nCheck if someting depends on this record";
        }

        return "Success";
    }

}
