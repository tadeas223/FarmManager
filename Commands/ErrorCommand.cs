public class ErrorCommand : IDBCommand
{
    public string Execute(string[] args, Database database)
    {
        if(args.Length < 2)
        {
            return "ErrorCommand: Missing Argument"; 
        }
        else
        {
            string str = "";
            for(int i = 1; i < args.Length; i++)
            {
                str += args[i];
            }
            return str;
        }
    }
}
