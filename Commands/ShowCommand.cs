public class ShowCommand : IDBCommand
{
    public string Execute(string[] args, Database database)
    {
        if(args.Length < 2)
        {
            return "ShowCommand: Missing Arguments";
        }
        else if(args.Length == 2)
        {
            return GetAll(args[1], database);
        }
        else if(args.Length == 3)
        {
            if(args[2] == string.Empty) return GetAll(args[1], database);

            int max;
            try
            {
                max = int.Parse(args[2]);
            }
            catch
            {
                return "Not a number";
            }

            return GetAllMax(args[1], max, database);
        }

        return "ShowCommand: Unknow error :(";

    }

    private string GetAll(string arg, Database database)
    {
        string str = "";
        switch(arg.ToLower())
        {
            case "associations":
                var arr1 = Association.GetAll(database);
                foreach(var a in arr1)
                {
                    str += a.ToString() + "\n";
                }
                break;
            case "crops":
                var arr2 = Crop.GetAll(database);
                foreach(var a in arr2)
                {
                    str += a.ToString() + "\n";
                }
                break;
            case "fields":
                var arr3 = Field.GetAll(database);
                foreach(var a in arr3)
                {
                    str += a.ToString() + "\n";
                }
                break;
            case "sowingrecords":
                var arr4 = SowingRecord.GetAll(database);
                foreach(var a in arr4)
                {
                    str += a.ToString() + "\n";
                }
                break;
            case "workers":
                var arr5 = Worker.GetAll(database);
                foreach(var a in arr5)
                {
                    str += a.ToString() + "\n";
                }
                break;
        }
        return str;
    }
    
    private string GetAllMax(string arg, int max, Database database)
    {
        string str = "";
        switch(arg.ToLower())
        {
            case "associations":
                var arr1 = Association.GetAll(database, max);
                foreach(var a in arr1)
                {
                    str += a.ToString() + "\n";
                }
                break;
            case "crops":
                var arr2 = Crop.GetAll(database, max);
                foreach(var a in arr2)
                {
                    str += a.ToString() + "\n";
                }
                break;
            case "fields":
                var arr3 = Field.GetAll(database, max);
                foreach(var a in arr3)
                {
                    str += a.ToString() + "\n";
                }
                break;
            case "sowingrecords":
                var arr4 = SowingRecord.GetAll(database, max);
                foreach(var a in arr4)
                {
                    str += a.ToString() + "\n";
                }
                break;
            case "workers":
                var arr5 = Worker.GetAll(database, max);
                foreach(var a in arr5)
                {
                    str += a.ToString() + "\n";
                }
                break;
        }
        return str;
    }
}
