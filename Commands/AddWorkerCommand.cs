public class AddWorkerCommand : IDBCommand
{
    public string Execute(string[] args, Database database)
    {
        Console.Write("name: ");
        string name = Console.ReadLine()!;
        
        Console.Write("surname: ");
        string surname = Console.ReadLine()!;

        Console.Write("birth date: ");
        string birthDateStr = Console.ReadLine()!;
        
        DateTime birthDate;
        try
        {
            birthDate = DateTime.Parse(birthDateStr);
        }
        catch
        {
            return "AddWorkerCommand: Not a date";
        }
        
        Console.Write("is fired: ");
        string firedStr = Console.ReadLine()!;
        
        bool fired;
        try
        {
            fired = bool.Parse(firedStr);
        }
        catch
        {
            return "AddWorkerCommand: Not a bool";
        }

        Console.Write("association name: ");
        string associationName = Console.ReadLine()!;
        
        Association association;
        try
        {
            association = Association.GetByName(associationName, database);
        }
        catch
        {
            return "AddWorkerCommand: Association not found";
        }

        Worker worker = new Worker(name, surname, birthDate, association, fired, database);
        worker.Insert();

        return "Success";
    }
}

