public class AddCropCommand : IDBCommand
{
    public string Execute(string[] args, Database database)
    {

        Console.Write("name: ");
        string name = Console.ReadLine()!;

        Console.Write("price: ");
        string priceStr = Console.ReadLine()!;

        float price;
        try
        {
            price = float.Parse(priceStr);
        }
        catch
        {
            return "AddCropCommand: Not a float";
        }

        Crop crop = new Crop(name, price, database);
        crop.Insert();
    
        return "Success";
    }
}
