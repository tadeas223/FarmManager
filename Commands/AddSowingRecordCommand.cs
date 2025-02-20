public class AddSowingRecordCommand : IDBCommand
{
    public string Execute(string[] args, Database database)
    {
        Console.Write("crop name: ");
        string cropName = Console.ReadLine()!;

        Crop crop;
        try
        {
            crop = Crop.GetByName(cropName, database);
        }
        catch
        {
            return "AddSowingRecordCommand: Crop not found";
        }
        
        Console.Write("field name: ");
        string fieldName = Console.ReadLine()!;

        Field field;
        try
        {
            field = Field.GetByName(fieldName, database);
        }
        catch
        {
            return "AddSowingRecordCommand: Field not found";
        }

    
        Console.Write("worker name: ");
        string workerName = Console.ReadLine()!;
        Console.Write("worker surname: ");
        string workerSurname = Console.ReadLine()!;
        
        Worker worker;
        try
        {
            worker = Worker.GetByName(workerName, workerSurname, database);
        }
        catch
        {
            return "AddSowingRecordCommand: Worker not found";
        }

        Console.Write("date: ");
        string dateStr = Console.ReadLine()!;

        DateTime date;
        try
        {
            date = DateTime.Parse(dateStr);
        }
        catch
        {
            return "AddSowingRecordCommand: Not a date";
        }

        SowingRecord record = new SowingRecord(crop, field, worker, date, database);
        record.Insert();

        return "Sucess";
    }
}
