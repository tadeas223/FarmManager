using System.Text.RegularExpressions;

public class UpdateWorkerCommand : IDBCommand
{
    public string Execute(string[] args, Database database)
    {
        Console.Write("name: ");
        string wName = Console.ReadLine()!;
        Console.Write("surname: ");
        string wSurname = Console.ReadLine()!;
        
        Worker worker;
        try
        {
            worker = Worker.GetByName(wName, wSurname, database);
        }
        catch
        {
            return "UpdateWorkerCommand: Worker not found";
        }

        string name = worker.Name;
        string surname = worker.Surname;
        DateTime birthDate = worker.BirthDate;
        Association association = worker.Association;
        bool fired = worker.Fired;

        Console.WriteLine("Specify what to update, devide with ,");
        Console.Write("update: ");
        
        string[] split = Regex.Split(Console.ReadLine()!.ToLower(), "[,][ ]?");
        
        foreach(string s in split)
        {
            if(s.Equals("birthDate"))
            {
                Console.Write("new birthDate: ");
                try
                {
                    birthDate = DateTime.Parse(Console.ReadLine()!);
                }
                catch
                {
                    return "UpdateWorkerCommand: Not a date";
                }
            }
            if(s.Equals("association"))
            {
                Console.Write("new association name: ");
                try
                {
                    association = Association.GetByName(Console.ReadLine()!, database);
                }
                catch
                {
                    return "UpdateWorkerCommand: Association not found";
                }
            }
            if(s.Equals("name"))
            {
                Console.Write("new name: ");
                name = Console.ReadLine()!;
            }
            if(s.Equals("surname"))
            {
                Console.Write("new surname: ");
                surname = Console.ReadLine()!;
            }
            if(s.Equals("fired"))
            {
                Console.Write("is fired: ");
                try
                {
                    fired= bool.Parse(Console.ReadLine()!);
                }
                catch
                {
                    return "UpdateWorkerCommand: Not a bool";
                }
            }
        }

        try
        {
            worker.Update(name, surname, birthDate, association, fired);
        }
        catch
        {
            return "UpdateWorkerCommand: Failed to update Worker";
        }
        
        return "Success";
    }
}
