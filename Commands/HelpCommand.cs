public class HelpCommand : IDBCommand
{
    public string Execute(string[] args, Database database)
    {
        return "help                - Print this text\n" + 
               "error [message]     - Prints the message back\n" +
               "exit                - Safely close the program\n" +
               "show [table] [max]? - Shows conents of the table\n" +
               "table-info          - Get brief info about all tables\n\n" +
               "add-field           - Adds Field record\n" +
               "add-crop            - Adds Crop record\n" +
               "add-worker          - Adds Worker record\n" +
               "add-association     - Adds Association record\n" +
               "add-sowing-record   - Adds SowingRecord\n\n" +
               "remove-field           - Removes Field record\n" +
               "remove-crop            - Removes Crop record\n" +
               "remove-worker          - Removes Worker record\n" +
               "remove-association     - Removes Association record\n" +
               "remove-sowing-record   - Removes SowingRecord\n\n" + 
               "update-field           - Updates date of a  Field record\n" +
               "update-crop            - Updates date of a  Crop record\n" +
               "update-worker          - Updates date of a  Worker record\n" +
               "update-association     - Updates date of a  Association record\n" +
               "update-sowing-record   - Updates date of a  SowingRecord\n";
    }
}
