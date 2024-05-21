using Newtonsoft.Json.Linq;
using PoeStashSearcher2023;

public class Program
{
    private const string POESESSID = "POESESSIDHERE";

    public static async Task Main()
    {
        // Refresh tabs and timeless jewels in stash?
        // Params: accountName, league (Solo%20Self-Found, SSF%20Necropolis)
        // Valid leagues: https://www.pathofexile.com/api/leagues (Solo Self-Found --> Solo%20Self-Found, SSF Necropolis --> SSF%20Necropolis)
        // Only need to run whenever you need to update stash, NOT every call. Comment it out after first call
        // Sleeps 1 minute per tab for rate-limiting prevention
        // await RefreshTabs("azCharlie", "Solo%20Self-Found", new []{"t0", "t2"});
        await RefreshTabs("azCharlie", "Solo%20Self-Found", new []{"t2"});
        
        
        // Check for PoE Ninja Jewels?
        // await CheckForPopularJewels();
        
        
        // Run personalized filter?
        RunPersonalizedFilter();
    }

    private static void RunPersonalizedFilter()
    {
        // Don't change these lines. Prep work
        var tabIds = GetListOfTabsFromFile();
        var allJewels = parseTabs(tabIds);
        var filter = new TimelessFilter(allJewels);

        // Uncomment or make a new filter, copying the format below.
        
        // TS Omni ranger location
        var allocatedNodes = new[]
        {
            "Quickstep", "Weapon Artistry", "Aspect of the Lynx", "Forces of Nature", "Multishot", "Heartseeker", 
            "Fervour", "Acuity", "Herbalism", "Winter Spirit", "King of the Hill", "Master Fletcher",
            "Inveterate", "Intuition", "Perfectionist"
        };
        var wantedMods = new Dictionary<string, int>() { { "+_double_damage", 5 }, { "+_intimidate", 7 }, { "+_percent_strength", 3 } };
        filter.Search("Lethal Pride", "Any", allocatedNodes, wantedMods);

        // Wardloop ranger
        // filter.Search("Brutal Restraint", "Balbala",
        // new[] {"Inveterate", "Heartseeker", "Intuition", "Flash Freeze", "Quickstep", "Herbalism"},
        // new Dictionary<string, int>() { { "+_flask_charges", 5 }, { "+_percent_dexterity", 3 }, { "+_onslaught", 10 } });

        // Wardloop scion location
        // var allocatedNodes = new[] { "Reflexes", "Window of Opportunity", "Battle Rouse", "Constitution"};
        // var wantedMods = new Dictionary<string, int>() { { "+_flask_charges", 5 }, { "+_percent_dexterity", 3 }, { "+_onslaught", 10 } };
        // filter.Search("Brutal Restraint", "Balbala", allocatedNodes, wantedMods);

        // Wardloop witch location
        // var allocatedNodes = new[] { "Cruel Preparation", "Infused Flesh", "Breath of Rime", "Heart of Ice"};
        // var wantedMods = new Dictionary<string, int>() { { "+_flask_charges", 5 }, { "+_percent_dexterity", 3 }, { "+_onslaught", 10 } };
        // filter.Search("Brutal Restraint", "Balbala", allocatedNodes, wantedMods);

        // Eye of winter arcanist brand pf
        // var allocatedNodes = new[] {"Inveterate", "Perfectionist", "Herbalism"};
        // var wantedMods = new Dictionary<string, int>() { { "devotion", 5 } };

        // Cold reap inquis
        // var allocatedNodes = new[] {"Devotion", "Endurance", "Overcharge", "Faith and Steel", "Divine Fervour", "Holy Dominion", "Light of Divinity"};
        // var wantedMods = new Dictionary<string, int>() { { "cult of lightning", 10 }, { "revitalising frost", 10 }, { "temple paths", 5 }, {"thaumaturgical aptitude", 5}};
        // filter.Search("Glorious Vanity", "Doryani", allocatedNodes, wantedMods);

        // Strength stack
        // var allocatedNodes = new[] {"Golem's Blood", "Cloth and Chain", "Savagery", "Defiance", "Fury Bolts", "Art of the Gladiator", "Bravery", "Master of the Arena"};
        // var wantedMods = new Dictionary<string, int>() { { "+_percent_strength", 5 }, { "+_warcry_buff_effect", 8 } };
        // filter.Search("Lethal Pride", "Any", allocatedNodes, wantedMods);

        // Strength stack
        // var allocatedNodes = new[] {"Golem's Blood", "Art of the Gladiator", "Bravery", "Master of the Arena"};
        // var wantedMods = new Dictionary<string, int>() { { "+_percent_strength", 5 }, { "+_warcry_buff_effect", 8 } };
        // filter.Search("Lethal Pride", "Any", allocatedNodes, wantedMods);

        // Strength stack
        // var allocatedNodes = new[] {"Exceptional Performance", "Sentinel", "Path of the Warrior", "Constitution"};
        // var wantedMods = new Dictionary<string, int>() { { "+_percent_strength", 5 }, { "+_warcry_buff_effect", 8 } };
        // filter.Search("Lethal Pride", "Any", allocatedNodes, wantedMods);

        // Manaforged arrows
        // var allocatedNodes = new[] {"Shaper", "Forethought", "Foresight", "Constitution"};
        // var wantedMods = new Dictionary<string, int>() { { "+_double_damage", 5 }, { "+_percent_strength", 5 }, { "+_strength", 3 } };
        // filter.Search("Lethal Pride", "Any", allocatedNodes, wantedMods);

        // Manaforged arrows templar/scion spot
        // var allocatedNodes = new[] {"Sanctity", "Expertise", "Ancestral Knowledge", "Dynamo"};
        // var wantedMods = new Dictionary<string, int>() { { "+_double_damage", 5 }, { "+_percent_strength", 5 }, { "+_strength", 3 }, { "+_warcry_buff_effect", 5 } };
        // filter.Search("Lethal Pride", "Any", allocatedNodes, wantedMods);

        // mana stack witch location
        // var allocatedNodes = new[] {"Deep Thoughts", "Prodigal Perfection", "Arcane Will"};
        // var wantedMods = new Dictionary<string, int>() { { "+_double_damage", 5 }, { "+_percent_strength", 5 }, { "+_strength", 3 }, { "+_warcry_buff_effect", 5 } };
        // filter.Search("Lethal Pride", "Any", allocatedNodes, wantedMods);

        // zhp penance Glorious Vanity
        // var allocatedNodes = new[] {"Arcanist's Dominion", "Fire Walker"};
        // var wantedMods = new Dictionary<string, int>() { { "flesh to lightning", 50 }, { "commanding presence", 5 }, { "thaumaturgical aptitude", 5 } };
        // filter.Search("Glorious Vanity", "Doryani", allocatedNodes, wantedMods);

        // forbidden rite occy Glorious Vanity
        // var allocatedNodes = new[] {"Expertise", "Ancestral Knowledge", "Dynamo"};
        // var wantedMods = new Dictionary<string, int>() { { "ritual of shadows", 50 }, { "commanding presence", 5 }, { "revitalising darkness", 5 } };
        // filter.Search("Glorious Vanity", "Xibaqua", allocatedNodes, wantedMods);

        // Minion life stack
        // var allocatedNodes = new[] {"Nimbleness", "Steeped in the Profane", "Undertaker", "Tolerance", "Melding", "Deep Wisdom"};
        // var wantedMods = new Dictionary<string, int>() { { "gleaming legion", 5 }, { "axiom warden", 8 } };
        // filter.Search("Elegant Hubris", "Any", allocatedNodes, wantedMods);

        // MF shadow location
        // var allocatedNodes = new[] {"Mind Drinker", "Circle of Life", "Infused", "Frenetic", "Clever Thief", 
        //     "From the Shadows", "Soul Thief", "Depth Perception", "Blood Drinker", "Mark the Prey", "Will of Blades", "Sleight of Hand", "Master of Blades"};
        // var wantedMods = new Dictionary<string, int>() { { "lioneye's focus", 5 }, {"discerning taste", 15} };
        // filter.Search("Elegant Hubris", "Any", allocatedNodes, wantedMods);

        // Dex stack ranger location
        // var allocatedNodes = new[] {"Inveterate", "Forces of Nature", "Herbalism", "King of the Hill", "Master Fletcher", "Intuition", "Quickstep", "Weapon Artistry", "Aspect of the Lynx", "Acuity"};
        // var wantedMods = new Dictionary<string, int>() { { "+_percent_dexterity", 5 }, { "+_dexterity", 2 } };
        // filter.Search("Brutal Restraint", "Any", allocatedNodes, wantedMods);

        // Frenzy stack slayer
        // var allocatedNodes = new[] {"Avatar of the Hunt", "Crystal Skin", "Weathered Hunter", "Deadly Draw", "Gladiator's Perseverance", "Burning Brutality", "Art of the Gladiator"};
        // var wantedMods = new Dictionary<string, int>() { { "eternal bloodlust", 5 }, { "eternal dominance", 5 } };
        // filter.Search("Elegant Hubris", "Any", allocatedNodes, wantedMods);

        // Armour stack aura effect
        // var allocatedNodes = new[] {"Expertise", "Ancestral Knowledge", "Righteous Fury", "Sanctity", "Sanctuary", "Combat Stamina"};
        // var wantedMods = new Dictionary<string, int>() { { "superiority", 5 }, {"gleaming legion", 2} };
        // filter.Search("Elegant Hubris", "Any", allocatedNodes, wantedMods);

        // Armour stack aura effect
        // var allocatedNodes = new[] {"Barbarism", "Stamina", "Juggernaut"};
        // var wantedMods = new Dictionary<string, int>() { { "superiority", 5 }, {"gleaming legion", 2} };
        // filter.Search("Elegant Hubris", "Any", allocatedNodes, wantedMods);

        // Armour stack aura effect
        // var allocatedNodes = new[] {"Barbarism", "Stamina", "Juggernaut"};
        // var wantedMods = new Dictionary<string, int>() { { "+_aura_effect", 5 } };
        // filter.Search("Brutal Restraint", "Any", allocatedNodes, wantedMods);

        // Immortal PF Phys to fire
        // var allocatedNodes = new[]
        // {
            // Shadow location
            // "Expeditious Munitions", "Arcing Blows", "Soul Thief", "Saboteur", "Elemental Focus", "Will of Blades", "Sleight of Hand", "Fangs of the Viper", "Resourcefulness", "Coldhearted Calculation", "Mind Drinker", "Circle of Life", "Infused", "Frenetic", "Blood Drinker", "Depth Perception", "Master of Blades", "Clever Thief", "From the Shadows", "Mark the Prey"
            // Scion ranger/shadow
            // "Master Sapper", "Overcharged", "Wasting", "Dire Torment", "Adder's Touch", "Taste for Blood", "Void Barrier", "Revenge of the Hunted", "Excess Sustenance", "Charisma", "Replenishing Remedies", "Ballistics"
            // Ranger
            // "Perfectionist", "Multishot", "Forces of Nature", "Inveterate", "Silent Steps", "Trick Shot", "Survivalist", "Aspect of the Lynx", "Weapon Artistry", "Intuition", "Quickstep", "Acuity", "Fervour", "Heartseeker", "Herbalism", "Winter Spirit", "Flash Freeze", "Swift Venoms"
            // Scion witch/shadow
            // "Tolerance", "Undertaker", "Steeped in the Profane", "Grave Intentions", "Vampirism", "Melding", "Deep Wisdom"
            // Witch
            // "Frost Walker", "Lord of the Dead", "Arcane Will", "Wandslinger", "Mystic Bulwark", "Prodigal Perfection", "Enigmatic Defence", "Cruel Preparation", "Deep Thoughts", "Infused Flesh", "Discord Artisan", "Presage", "Malicious Intent", "Golem Commander", "Breath of Flames", "Heart of Thunder", "Breath of Lightning", "Breath of Rime", "Heart of Ice"
            // Witch/shadow cluster
            // "Light Eater", "Searing Heat", "Winter's Embrace", "Throatseeker", "Alacrity", "Physique", "Conjured Barrier", "Mysticism", "Efficient Explosives", "High Voltage", "Arcane Sanctuary", "Influence", "Whispers of Doom", "Fusillade", "Disintegration"
            // Ranger cluster
            // "Clever Thief", "Mark the Prey", "Hunter's Gambit", "Master of Blades", "Perfectionist", "Forces of Nature", "Silent Steps", "Survivalist", "Heartseeker", "Inveterate", "Multishot", "Careful Conservationist", "From the Shadows", "Fatal Toxins", "Piercing Shots", "Split Shot", "Trick Shot"
            // Scion right
            // "Foresight", "Path of the Savant", "Potency of Will", "Thrill Killer", "Destructive Apparatus", "Leadership", "Potent Connections", "True Strike", "Hired Killer", "Master Sapper", "Path of the Hunter", "Reflexes", "Exceptional Performance", "Window of Opportunity"
            // Scion bottom
            // "Hired Killer", "Path of the Hunter", "Reflexes", "Arcane Chemistry", "Window of Opportunity", "Exceptional Performance", "Arcane Chemistry", "Totemic Zeal", "Battle Rouse", "Path of the Warrior", "Constitution", "Sentinel", "Skittering Runes"
            // Scion left
            // "Decay Ward", "Dreamer", "Forethought", "Foresight", "Path of the Savant", "Shaper", "Relentless", "Veteran Soldier", "Inspiring Bond", "Skittering Runes", "Potency of Will", "Potent Connections", "Constitution", "Path of the Warrior", "Totemic Zeal"
            // Scion templar/witch
            // "Arcanist's Dominion", "Fire Walker", "Enduring Bond", "Quick Recovery", "Essence Extraction", "Anointed Flesh", "Asylum", "Essence Infusion"
            // Shadow
            // "Mind Drinker", "Fangs of the Viper", "Blood Drinker", "Clever Thief"
            // Ranger poison forbidden rite
            // "Charisma", "Wasting", "Revenge of the Hunted", "Ballistics"

            // Templar
            // "Faith and Steel", "Devotion", "Endurance"

            // Deadeye KB Mahuxotl's
            // "Forces of Nature", "Aspect of the Lynx", "Weapon Artistry", "Quickstep", "Intuition", "Herbalism", "Acuity", "Heartseeker", "Inveterate", "Multishot", "Trick Shot"
        // };
        // var wantedMods = new Dictionary<string, int>() { { "+_physical_taken_as_fire", 5 } };
        // filter.Search("Lethal Pride", "Rakiata", allocatedNodes, wantedMods);
        
        // Spell elegant hubris
        // var allocatedNodes = new[]
        // {
        //     "Quick Recovery", "Anointed Flesh", "Asylum", "Essence Extraction"
        // };
        // var wantedMods = new Dictionary<string, int>() { { "brutal execution", 5 }, { "dialla's wit", 5 }, { "flawless execution", 5 }, { "gemling ambush", 5 }, { "gemling inquisition", 5 }, { "superiority", 5 } };
        // filter.Search("Elegant Hubris", "Any", allocatedNodes, wantedMods);
        
        // Cremation/DD/Unearth occy jewel shadow location
        // var allocatedNodes = new[] {"Clever Thief", "Infused", "Frenetic", "Blood Drinker", "Soul Thief", "Mind Drinker"};
        // var wantedMods = new Dictionary<string, int>() { { "ritual of shadows", 5 }, { "commanding presence", 5 }, { "revitalising darkness", 5} };
        // filter.Search("Glorious Vanity", "Doryani", allocatedNodes, wantedMods);

        // Cremation/DD/Unearth occy jewel witch location
        // var allocatedNodes = new[] {"Lord of the Dead", "Instability", "Mystic Bulwark", "Deep Thoughts", "Cruel Preparation"};
        // var wantedMods = new Dictionary<string, int>() { { "ritual of shadows", 5 }, { "commanding presence", 5 } };
        // filter.Search("Glorious Vanity", "Doryani", allocatedNodes, wantedMods);
    }
    
    static async Task CheckForPopularJewels()
    {
        // Refresh ladder data?
        // await Characters.FetchLadderData(POESESSID, 150);
        
        // Refresh all characters?
        // await Characters.FetchCharacterData();
        
        // Find common timeless jewels in the character files
        // var commonJewels = await Characters.FindCommonJewels();

        var popularJewels = Characters.ParseSavedFile();
        var tabIds = GetListOfTabsFromFile();
        
        // Refresh tabs?
        // await QueryTabs(POESESSID, tabIds, new [] {"t1", "t2", "t3", "t0"});
        
        var allJewels = parseTabs(tabIds);

        var foundOne = false;
        foreach (var jewel in allJewels)
        {
            var index = popularJewels.IndexOf(jewel);
            if (index != -1)
            {
                // Correct seed, but wrong conqueror
                if (popularJewels[index].AllConqueredByTheSame && jewel.ConqueredBy != popularJewels[index].ConqueredBy)
                {
                    continue;
                }

                foundOne = true;
                
                Console.WriteLine(jewel.TabName + " > " + jewel.Type + " | " + jewel.ConqueredBy + " | " + jewel.Number + " | " + popularJewels[index].Occurrences);
                if (popularJewels[index].AllConqueredByTheSame)
                {
                    Console.WriteLine("     All are " + popularJewels[index].ConqueredBy);
                }
                else
                {
                    Console.WriteLine("     Conqueror doesn't seem to matter");
                }
                foreach (var link in popularJewels[index].poeninjaLinks)
                {
                    Console.WriteLine("     " + link);
                }
            }
        }

        if (!foundOne)
        {
            Console.WriteLine("Didn't find any matching jewels :(");
        }
    }
    
    // ***********************************************************************************************
    // Don't change any code below this
    // ***********************************************************************************************
    
    public static async Task RefreshTabs(string accountName, string league, string[] whichTabs)
    {
        // Writes all tabs to file
        await WriteToTabsListFile(accountName, league, POESESSID);
        
        // Get all tab ids from that file
        var tabIds = GetListOfTabsFromFile();

        await QueryTabs(POESESSID, tabIds, whichTabs);
    }

    static List<TimelessJewel> parseTabs(string[] tabIds)
    {
        var list = new List<TimelessJewel>();

        for (var j = 0; j < tabIds.Length; j += 2)
        {
            var tabId = tabIds[j + 1];

            var path = Utility.GetResourcesFilePath();
            var filePath = Path.Join(path, "tabs", tabId + ".txt");

            if (!File.Exists(filePath))
            {
                continue;
            }

            var fileContent = File.ReadAllText(filePath);

            // Only look at stash if it contains the string "Timeless Jewel"
            if (!fileContent.Contains("\"Timeless Jewel\""))
                continue;

            var json = JObject.Parse(fileContent);
            var stash = (JObject)json["stash"]!;

            var currentTabName = stash["name"]!.ToString();

            var items = stash["items"];

            foreach (var item in items!)
            {
                if (item["typeLine"]!.ToString() != "Timeless Jewel") continue;

                var explicitMods = item["explicitMods"]!;
                var line = explicitMods[0]!.ToString().Split("\n")[0];

                // Example Lethal Pride
                var name = item["name"]!.ToString();

                var words = line.Split(" ");

                // Example Akoya
                var conqueredUnder = words[^1];

                // Number
                var number = 0;
                for (var i = 1; i < words.Length; i++)
                {
                    if (int.TryParse(words[i], out var val))
                    {
                        number = val;
                    }
                }

                var jewel = new TimelessJewel(tabId, currentTabName, name, conqueredUnder, number);
                list.Add(jewel);
            }
        }

        return list;
    }

    static async Task QueryTabs(string POESESSID, string[] tabIds, string[] tabNames)
    {
        var query = new Query(POESESSID);
        var count = 0;
        for (var i = 0; i < tabIds.Length; i += 2)
        {
            var tabId = tabIds[i + 1];
            var tabName = tabIds[i];

            var path = Utility.GetResourcesFilePath();
            var filePath = Path.Join(path, "tabs", tabId + ".txt");

            if (!tabNames.Contains(tabName))
                continue;

            // If file exists, tab has already been queried. Continue
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            // Run query
            var getStashUrl = $"https://api.pathofexile.com/stash/Solo%20Self-Found/{tabId}";
            var stashContent = await query.Get(getStashUrl);
            if (string.IsNullOrWhiteSpace(stashContent))
            {
                continue;
            }

            // Create file
            var file = File.Create(filePath);
            file.Close();

            // Otherwise, content found. Save it to the file
            using (var writer = new StreamWriter(filePath))
            {
                writer.WriteLine(stashContent);
            }

            Console.WriteLine("Finished parsing tab " + tabNames[count] + ": " + ++count + " / " + tabNames.Length + " done");
            if (tabIds.Length != 1 && count != tabNames.Length)
            {
                Thread.Sleep(60000);
            }
        }
    }

    static async Task WriteToTabsListFile(string accountName, string league, string POESESSID)
    {
        var query = new Query(POESESSID);
        var url =
            $"https://pathofexile.com/character-window/get-stash-items?accountName={accountName}&realm=pc&league={league}&tabs=1&tabIndex=0";
        var content = await query.Get(url);
        var json = JObject.Parse(content);

        var tabs = json["tabs"];

        var path = Utility.GetResourcesFilePath();

        if (Directory.Exists(Path.Join(path, "tabs")))
        {
            Directory.Delete(Path.Join(path, "tabs"), true);
        }

        Directory.CreateDirectory(Path.Join(path, "tabs"));

        var filePath = Path.Join(path, "tabsList.txt");
        // Write file using StreamWriter
        using (var writer = new StreamWriter(filePath))
        {
            foreach (var tab in tabs)
            {
                writer.WriteLine(tab["n"]!.ToString());
                writer.WriteLine(tab["id"]!.ToString());
            }
        }
    }

// Looks at tabIds.txt, returns an array of each
    static string[] GetListOfTabsFromFile()
    {
        var path = Utility.GetResourcesFilePath();
        var filePath = Path.Join(path, "tabsList.txt");
        var readText = File.ReadAllText(filePath);
        var split = readText.Split("\r\n").ToList();
        // split.RemoveAll(string.IsNullOrWhiteSpace);
        if (string.IsNullOrEmpty(split[^1]))
        {
            split.RemoveAt(split.Count - 1);
        }

        return split.ToArray();
    }
}