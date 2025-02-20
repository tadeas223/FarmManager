public class TableInfoCommand : IDBCommand
{
    public string Execute(string[] args, Database database)
    {
        return "Crops         - name, price\n" +
               "Fields        - name, Association\n" + 
               "Associations  - name\n" + 
               "Workers       - name, surname, birthDate, fired\n" + 
               "SowingRecords - Field, Association, Worker, date\n";
    }
}
