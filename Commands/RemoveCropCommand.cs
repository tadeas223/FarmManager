public class RemoveCropCommand : IDBCommand
{
    public string Execute(string[] args, Database database)
    {
        Console.Write("name: ");
        string name = Console.ReadLine()!;

        Crop crop;
        try
        {
            crop = Crop.GetByName(name, database);
        }
        catch
        {
            return "RemoveCropCommand: Crop not found"; 
        }

        try
        {
            crop.Remove();
        }
        catch
        {
            return "RemoveCropCommand: Failed to remove Crop";
        }

        return "Success";
    }
}
