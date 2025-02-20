public class AddAssociationCommand : IDBCommand
{
    public string Execute(string[] args, Database database)
    {
        Console.WriteLine("name: ");
        string name = Console.ReadLine()!;

        Association association = new Association(name, database);
        association.Insert();

        return "Success";
    }

}
