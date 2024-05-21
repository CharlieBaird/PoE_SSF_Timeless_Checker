namespace PoeStashSearcher2023;

public class SeedManager
{
    public List<Seed> AllSeeds { get; set; }
    
    public SeedManager(string type)
    {
        var path = AppDomain.CurrentDomain.BaseDirectory;
        var filePath = Path.Join(path, "csv_data", type.Replace(" ", "") + ".csv");
        var fileContent = File.ReadAllText(filePath);

        var lines = fileContent.Split("\n");
        AllSeeds = new List<Seed>();

        var nodesList = lines[0].Split(",");
        for (var i = 2; i < nodesList.Length; i++)
        {
            nodesList[i] = nodesList[i].Replace("\"", "");
        }

        for (var i = 1; i < lines.Length; i++)
        {
            var lineSplit = lines[i].Split(",");
            var seed = new Seed(int.Parse(lineSplit[0]));
            
            // foreach column starting at 2
            for (var j = 2; j < lineSplit.Length; j++)
            {
                if (j > nodesList.Length - 1) break;
                
                var node = nodesList[j].ToLower();
                var replacement = lineSplit[j].ToLower();
                
                seed.Nodes.Add(node, replacement);
            }
            
            AllSeeds.Add(seed);
        }
    }

    public int ParseJewel(TimelessJewel jewel, string[] allocatedNodes, Dictionary<string, int> wantedMods)
    {
        for (var i = 0; i < allocatedNodes.Length; i++)
        {
            allocatedNodes[i] = allocatedNodes[i].ToLower();
        }

        if (jewel.Number == 11479)
        {
            Console.WriteLine();
        }
        
        jewel.Vals = new Dictionary<string, int>();
        
        var score = 0;
        foreach(var seed in AllSeeds)
        {
            if (seed.Value == jewel.Number)
            {
                
                foreach (var allocNode in allocatedNodes)
                {
                    string val = "";
                    try
                    {
                        val = seed.Nodes[allocNode];
                    }
                    catch (KeyNotFoundException e)
                    {
                        Console.WriteLine(e.Message);
                        Environment.Exit(0);
                    }

                    if (wantedMods.Keys.Contains(val))
                    {
                        if (!jewel.Vals.ContainsKey(val))
                        {
                            jewel.Vals.Add(val, 0);
                        }
                        jewel.Text += allocNode + ", ";
                        
                        jewel.Vals[val]++;
                        score += wantedMods[val];
                    }
                }
                
                break;
            }
        }

        return score;
    }
}

public class Seed
{
    public int Value { get; set; }
    public Dictionary<string, string> Nodes { get; set; }

    public Seed(int value)
    {
        Value = value;
        Nodes = new Dictionary<string, string>();
    }
}