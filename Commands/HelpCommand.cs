public class HelpCommand : IDBCommand
{
    public string Execute(string[] args, Database database)
    {
        return "help                - Print this text\n" + 
               "error [message]     - Prints the message back\n" +
               "exit                - Safely close the program\n" +
               "show [table] [max]? - Shows info from the table\n" +
               "table-info          - Get brief info about all tables\n\n" +
               "add-field           - Adds Field record\n" +
               "add-crop            - Adds Crop record\n" +
               "add-worker          - Adds Worker record\n" +
               "add-association     - Adds Association record\n" +
               "add-sowing-record   - Adds SowingRecord\n";
    }
}
