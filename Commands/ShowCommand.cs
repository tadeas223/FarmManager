public class ShowCommand : IDBCommand
{
    public string Execute(string[] args, Database database)
    {
        if(args.Length >= 3)
        {
            int max;
            try
            {
                max = int.Parse(args[2]);
            }
            catch
            {
                return "ShowCommand: Not a number";
            }

            string str = "";
            switch(args[1].ToLower())
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
        else if(args.Length < 2)
        {
            return "Missing table name";
        }
        else
        {
            string str = "";
            switch(args[1].ToLower())
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

    }
}
