public class RemoveSowingRecordCommand : IDBCommand
{
    public string Execute(string[] args, Database database)
    {
        Console.Write("Field name: ");
        string fieldName = Console.ReadLine()!;

        Console.Write("date: ");
        string dateStr = Console.ReadLine()!;
        
        DateTime date;
        try
        {
            date = DateTime.Parse(dateStr);
        }
        catch
        {
            return "RemoveSowingRecordCommand: Not a date"; 
        }
        
        Field field;
        try
        {
            field = Field.GetByName(fieldName, database);
        }
        catch
        {
            return "RemoveSowingRecordCommand: Field not found";
        }

        SowingRecord record;
        try
        {
            record = SowingRecord.GetByFieldDate(field, date, database); 
        }
        catch
        {
            return "RemoveSowingRecordCommand: SowingRecord not found";
        }
        
        try
        {
            record.Remove();
        }
        catch
        {
            return "RemoveSowingRecordCommand: Failed to remove SowingRecord\nCheck if something depends on this record";
        }
        return "Success";
    }
}
