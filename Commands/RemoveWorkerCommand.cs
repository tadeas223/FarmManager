public class RemoveWorkerCommand : IDBCommand
{
    public string Execute(string[] args, Database database)
    {
        Console.Write("name: ");
        string name = Console.ReadLine()!;

        Console.Write("surname: ");
        string surname = Console.ReadLine()!;
    
        Worker worker;
        try
        {
            worker = Worker.GetByName(name, surname, database);
        }
        catch
        {
            return "RemoveWorkerCommand: Worker not found"; 
        }
        
        try
        {
            worker.Remove();
        }
        catch
        {
            return "RemoveWorkerCommand: Failed to remove Worker\nCheck if something depends on this record";
        }
        return "Success";
    }
}
