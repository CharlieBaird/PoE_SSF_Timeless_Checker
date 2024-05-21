namespace PoeStashSearcher2023;

public class TimelessJewel : IComparable
{
    public TimelessJewel(string tabId, string tabName, string type, string conqueredBy, int number)
    {
        TabId = tabId;
        TabName = tabName;
        Type = type;
        ConqueredBy = conqueredBy;
        Number = number;
    }

    public string TabId { get; set; }
    public string TabName { get; set; }
    public string Type { get; set; }
    public string ConqueredBy { get; set; }
    public int Number { get; set; }
    public string Text { get; set; }
    public int Occurrences { get; set; }

    public List<string> poeninjaLinks = new List<string>();

    public bool AllConqueredByTheSame = true;
    
    public Dictionary<string, int> Vals { get; set; }

    public int Score { get; set; }

    public void DebugPrint()
    {
        Console.WriteLine(TabId + " > " + TabName + " > " + Type + " | " + ConqueredBy + " | " + Number);
    }
    
    public void Print()
    {
        Console.WriteLine(Score + " " +  TabName + " > " + Type + " | " + ConqueredBy + " | " + Number);
        if (Vals.Count > 0)
        {
            foreach (var (key, val) in Vals)
            {
                Console.Write("     " + key + ": " + val);
            }
            Console.WriteLine();
            Console.WriteLine("     " + Text[..^2]);
        }
    }

    public int CompareTo(object? obj)
    {
        var other = (TimelessJewel)obj!;

        return this.Score - other.Score;
    }

    public override bool Equals(object that)
    {
        var other = (TimelessJewel) that;
        return this.Number == other.Number && this.Type == other.Type;
    }

    public static string parseConqueredByForType(string name)
    {
        switch (name)
        {
            case "Caspiro":
            case "Cadiro":
            case "Victario":
                return "Elegant Hubris";
            case "Asenath":
            case "Balbala":
            case "Nasima":
                return "Brutal Restraint";
            case "Ahuana":
            case "Doryani":
            case "Xibaqua":
                return "Glorious Vanity";
            case "Akoya":
            case "Kaom":
            case "Rakiata":
                return "Lethal Pride";
            case "Avarius":
            case "Dominus":
            case "Maxarius":
                return "Militant Faith";
            default:
                return null;
        }
    }
}