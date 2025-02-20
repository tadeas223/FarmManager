string credentialsFile = "credentials.json";

var credentials = DatabaseCredentials.FromJson(File.ReadAllText(credentialsFile));
Console.WriteLine(credentials);


