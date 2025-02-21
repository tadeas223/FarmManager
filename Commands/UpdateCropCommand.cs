using System.Text.RegularExpressions;

public class UpdateCropCommand : IDBCommand
{
    public string Execute(string[] args, Database database)
    {
        Console.Write("name: ");
        string cropName = Console.ReadLine()!;
        Crop crop;
        try
        {
            crop = Crop.GetByName(cropName, database);
        }
        catch
        {
            return "UpdateCropCommand: Crop not found";
        }

        string name = crop.Name;
        float price = crop.Price;

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
            if(s.Equals("price"))
            {
                Console.Write("new price: ");
                try
                {
                    price = float.Parse(Console.ReadLine()!);
                }
                catch
                {
                    return "UpdateCropCommand: Not a float";
                }
            }
        }


        try
        {
            crop.Update(name, price);
        }
        catch
        {
            return "UpdateCropCommand: Failed to update Crop";
        }

        return "Success";
    }
}
