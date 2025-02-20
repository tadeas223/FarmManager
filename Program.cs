string credentialsFile = "credentials.json";

var credentials = DatabaseCredentials.FromJson(File.ReadAllText(credentialsFile));

using(Database db = new Database(credentials, "test"))
{
    Crop crop = new Crop("pes", 123, db);
    crop.Insert();
    
    Console.WriteLine(crop.Id);
}
