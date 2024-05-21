namespace PoeStashSearcher2023;

public class TimelessFilter
{
    private List<TimelessJewel> Jewels { get; }
    public TimelessFilter(List<TimelessJewel> jewels)
    {
        Jewels = jewels;
    }

    public void Search(string type, string conqueredBy, string[] allocatedNodes, Dictionary<string, int> wantedNodes)
    {
        var foundJewels = new List<TimelessJewel>();
        var manager = new SeedManager(type);

        // Filter list of jewels
        for (var i = 0; i < Jewels.Count; i++)
        {
            var jewel = Jewels[i];

            if (jewel.Number == 15271)
            {
                Console.WriteLine();
            }
            
            
            if (jewel.Type != type || (conqueredBy != "Any" && conqueredBy != jewel.ConqueredBy))
            {
                Jewels.RemoveAt(i--);
                continue;
            }
            
            var score = manager.ParseJewel(jewel, allocatedNodes, wantedNodes);
            jewel.Score = score;
            foundJewels.Add(jewel);
        
            // jewel.DebugPrint();
        }

        foundJewels.Sort();
    
        foreach (var jewel in foundJewels)
        {
            jewel.Print();
            jewel.Score = 0;
        }
    }
}