/// <summary>
/// Utilities or sometning.
/// </summary>
public static class Utility
{
    /// <summary>
    /// Inserts 4 spaces at each new line of a string.
    /// </summary>
    /// <param name="tabs">Number of tabs to add at each line</param>
    /// <param name="original">String to add tabs in</param>
    public static string AddTabs(string original, int tabs)
    {
        List<string> list = new List<string>();

        string[] split = original.Split("\n");
        foreach(var s in split)
        {
            string str = s;
            for(int i = 0; i < tabs; i++)
            {
                str = "    " + str; 
            }

            list.Add(str);
        }
        
        string result = "";
        foreach(var s in list)
        {
            result += s + "\n";
        }

        return result;
    }
}
