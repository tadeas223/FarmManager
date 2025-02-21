/// <summary>
/// Class that contains functions for the console enviroment.
/// </summary>
public static class DatabaseConsole
{
    public static string CREDENTIALS_FILE = "credentials.json";
    
    private static Dictionary<string, IDBCommand>? commands = null;

    /// <summary>
    /// Starts the console enviroment.
    /// </summary>
    public static void Run()
    {
        Console.WriteLine(@"
            
            ************************************************************
            |   Farmer Database :)                                     |  
            ************************************************************
        
        ");
        
        Database db = new Database(GetCredentials());


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
    
    /// <summary>
    /// Executes a command from the console enviroment command list.
    /// </summary>
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
            throw e;
        //    return "Console: Uncaught exception " + e.Message;
        }


    }
    
    /// <summary>
    /// Creates a dictionary with all the console commands.
    /// </summary>
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

        commands.Add("remove-crop", new RemoveCropCommand());
        commands.Add("remove-field", new RemoveFieldCommand());
        commands.Add("remove-worker", new RemoveWorkerCommand());
        commands.Add("remove-association", new RemoveAssociationCommand());
        commands.Add("remove-sowing-record", new RemoveSowingRecordCommand());
        
        commands.Add("update-crop", new UpdateCropCommand());
        commands.Add("update-field", new UpdateFieldCommand());
        commands.Add("update-worker", new UpdateWorkerCommand());
        commands.Add("update-association", new UpdateAssociationCommand());
        commands.Add("update-sowing-record", new UpdateSowingRecordCommand());
        return commands;
    }
    
    /// <summary>
    /// Starts a dialog with the user to get creadentials for the database.
    /// Saves the credentials in a file.
    /// </summary>
    private static DatabaseCredentials GetCredentials()
    {
        DatabaseCredentials credentials = null!; 
        bool valid = false;
        while(!valid)
        {
            bool ask = false; 
            if(File.Exists(CREDENTIALS_FILE))
            {
                Console.Write("Load credentials [Yes/No]: ");
                string input = Console.ReadLine()!;
                if(input.ToCharArray()[0] == 'Y')
                {
                    credentials = DatabaseCredentials.FromJson(File.ReadAllText(CREDENTIALS_FILE));
                }
                else
                {
                    ask = true; 
                }
            
            }

            if(ask)
            {
                credentials = AskCredentials();
            }
            
            try
            {
                if(!Database.Exists(credentials))
                {
                    Console.WriteLine($"The database \"{credentials.DatabaseName}\" does not exist!!!");
                    Console.Write($"Create \"{credentials.DatabaseName}\" database [Yes/No]: ");
                    string input = Console.ReadLine()!;
                    if(input.ToCharArray()[0] == 'Y')
                    {
                        Database.Create(credentials);
                        valid = true;
                    }
                    else
                    {
                        Console.WriteLine("Missing database");
                    }
                }
                else
                {
                    valid = true;
                }
            }
            catch
            {
                Console.WriteLine("Failed to connect to the database");
            }
            
        }

        File.WriteAllText(CREDENTIALS_FILE, credentials.ToJson());

        return credentials;
    }
    
    /// <summary>
    /// Creates a dialog with the user to get his credentials.
    /// </summary>
    private static DatabaseCredentials AskCredentials()
    {
        Console.Write("database url: ");
        string databaseUrl = Console.ReadLine()!;
        Console.Write("username: ");
        string username = Console.ReadLine()!;
        Console.Write("password: ");
        string password = Console.ReadLine()!;
        Console.Write("database name: ");
        string databaseName = Console.ReadLine()!;
        
        // Create the credentials
        var credentials = new DatabaseCredentials(databaseUrl, databaseName, username, password);
        
        return credentials;
    }
}
