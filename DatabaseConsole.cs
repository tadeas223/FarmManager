public static class DatabaseConsole
{
    public static string CREDENTIALS_FILE = "credentials.json";
    
    private static Dictionary<string, IDBCommand>? commands = null;

    public static void Run()
    {
        Console.WriteLine(@"
            
            ************************************************************
            |   Farmer Database :)                                     |  
            ************************************************************
        
        ");
        
        DatabaseCredentials credentials = null!;
        string databaseName;
        
        // If credentials from previous session exists ask if the user wants to use them.
        // If yes set askCredentials to true
        bool askCredentials = true;
        if(File.Exists(CREDENTIALS_FILE))
        {
            Console.Write("Load credentials [Yes/No]: ");
            string input = Console.ReadLine()!;
            if(input.ToCharArray()[0] == 'Y')
            {
                credentials = DatabaseCredentials.FromJson(File.ReadAllText(CREDENTIALS_FILE));
                askCredentials = false; 
            }
        }
    
        // Ask the user for credentials and database name
        // + save the credentials
        if(askCredentials)
        {
            while(true)
            {
                Console.Write("database url: ");
                string databaseUrl = Console.ReadLine()!;
                Console.Write("username: ");
                string username = Console.ReadLine()!;
                Console.Write("password: ");
                string password = Console.ReadLine()!;
                Console.Write("database name: ");
                databaseName = Console.ReadLine()!;
                
                // Create the credentials
                credentials = new DatabaseCredentials(databaseUrl, databaseName, username, password);
                File.WriteAllText(CREDENTIALS_FILE, credentials.ToJson());

                if(!Database.Exists(credentials))
                {
                    Console.WriteLine($"The database \"{databaseName}\" does not exist!!!");
                    Console.Write($"Create \"{databaseName}\" database [Yes/No]: ");
                    string input = Console.ReadLine()!;
                    if(input.ToCharArray()[0] == 'Y')
                    {
                        Database.Create(credentials);
                        break;
                    }
                }
                break;
            }
        }
        
        using(var db = new Database(credentials))
        {
            Console.WriteLine(@"
                
            ************************************************************
            |   Welcome to the Database :)                             |  
            ************************************************************
            
            ");
            

            while(true)
            {
                Console.Write("> ");
                Console.WriteLine(Cmd(Console.ReadLine()!, db));               
            }
        }
    }

    public static string Cmd(string command, Database db)
    {
        if(commands == null) commands = GetCommands();
        string[] args =command.Split(" "); 
        try
        {
            return commands[args[0]].Execute(args, db);
        }
        catch (KeyNotFoundException)
        {
            return "Console: Command not found (try help)";
        }
        catch (Exception e)
        {

            return "Console: Uncaught exception " + e.Message;
        }


    }

    private static Dictionary<string, IDBCommand> GetCommands()
    {
        Dictionary<string, IDBCommand> commands = new Dictionary<string, IDBCommand>();
        commands.Add("help", new HelpCommand());
        commands.Add("error", new ErrorCommand());
        commands.Add("exit", new ExitCommand());
        commands.Add("show", new ShowCommand());
        commands.Add("table-info", new TableInfoCommand());

        commands.Add("add-crop", new AddCropCommand());
        commands.Add("add-field", new AddFieldCommand());
        commands.Add("add-worker", new AddWorkerCommand());
        commands.Add("add-association", new AddAssociationCommand());
        commands.Add("add-sowing-record", new AddSowingRecordCommand());

        return commands;
    }

}
