using System.Text.RegularExpressions;

public class UpdateSowingRecordCommand : IDBCommand
{
    public string Execute(string[] args, Database database)
    {
        Console.Write("field name: ");
        string fieldName = Console.ReadLine()!;

        Field sowField;
        try 
        {
            sowField = Field.GetByName(fieldName, database);
        }
        catch
        {
            return "UpdateSowingRecordCommand: Field not found";
        }
       
        Console.Write("date: ");
        string dateStr = Console.ReadLine()!;
        
        DateTime sowDate;
        try
        {
            sowDate = DateTime.Parse(dateStr);
        }
        catch
        {
            return "UpdateSowingRecordCommand: Not a date";
        }
        
        SowingRecord record;
        try
        {
            record = SowingRecord.GetByFieldDate(sowField, sowDate, database);
        }
        catch
        {
            return "UpdateSowingRecordCommand: SowingRecord not found";
        }

        Crop crop = record.Crop;
        Worker worker = record.Worker;
        Field field = record.Field;
        DateTime date = record.Date;

        Console.WriteLine("Specify what to update, devide with ,");
        Console.Write("update: ");
        
        string[] split = Regex.Split(Console.ReadLine()!.ToLower(), "[,][ ]?");
        
        foreach(string s in split)
        {
            if(s.Equals("date"))
            {
                Console.Write("new date: ");
                try
                {
                    date = DateTime.Parse(Console.ReadLine()!);
                }
                catch
                {
                    return "UpdateSowingRecordCommand: Not a date";
                }
            }
            if(s.Equals("worker"))
            {
                Console.Write("new worker name: ");
                string wName = Console.ReadLine()!; 
                Console.Write("new worker surname: ");
                string wSurname = Console.ReadLine()!; 
                try
                {
                    worker = Worker.GetByName(wName, wSurname, database);
                }
                catch
                {
                    return "UpdateCropCommand: Worker not found";
                }
            }
            if(s.Equals("field"))
            {
                Console.Write("new field name: ");
                try
                {
                    field = Field.GetByName(Console.ReadLine()!, database);
                }
                catch
                {
                    return "UpdateFieldCommand: Field not found";
                }
            }
            if(s.Equals("crop"))
            {
                Console.Write("new crop name: ");
                try
                {
                    crop = Crop.GetByName(Console.ReadLine()!, database);
                }
                catch
                {
                    return "UpdateFieldCommand: Crop not found";
                }
            }
        }

        return "Success";
    }
}
