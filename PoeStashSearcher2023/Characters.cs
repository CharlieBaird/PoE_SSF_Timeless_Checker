using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json.Linq;


namespace PoeStashSearcher2023;

public class Characters
{
    private static readonly HttpClient client = new HttpClient();

    public static async Task FetchLadderData(string POESESSID, int iterations)
    {
        int offset = 0;
        string baseUrl = "https://www.pathofexile.com/api/ladders/Necropolis?limit=100&offset=";
        
        var query = new Query(POESESSID);
        
        var path = Utility.GetResourcesFilePath();

        for (int i = 0; i < iterations; i++)
        {
            var filePath = Path.Join(path, "ladder", $"offset_{offset}.txt");
            string url = baseUrl + offset;
            try
            {
                var responseBody = await query.Get(url);
                
                // HttpResponseMessage response = await client.GetAsync(url);
                // response.EnsureSuccessStatusCode();
                // string responseBody = await response.Content.ReadAsStringAsync();

                // await File.WriteAllTextAsync(filePath, responseBody);
                
                // Create file
                var file = File.Create(filePath);
                file.Close();

                // Otherwise, content found. Save it to the file
                using (var writer = new StreamWriter(filePath))
                {
                    writer.WriteLine(responseBody);
                }

                Console.WriteLine($"Data written to {filePath}");
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
            }
            catch (IOException e)
            {
                Console.WriteLine($"File IO error: {e.Message}");
            }

            offset += 100;
            Thread.Sleep(5000); // Sleep for 5 seconds
        }
    }

    public static async Task FetchCharacterData()
    {
        var path = Utility.GetResourcesFilePath();
        var counter = 5802;
        for (var i = 9400; i < 15000; i+=100)
        {
            var filePath = Path.Join(path, "ladder", $"offset_{i}.txt");
            var json = await File.ReadAllTextAsync(filePath);
            
            JObject jsonObject = JObject.Parse(json);
            JArray entries = (JArray)jsonObject["entries"];


            foreach (var entry in entries)
            {
                string characterName = (string)entry["character"]["name"];
                string accountName = (string)entry["account"]["name"];
                Console.WriteLine($"Character Name: {characterName}, Account Name: {accountName}");
                
                if (characterName.Contains('?') || accountName.Contains('?')) continue;
                
                string url = $"https://poe.ninja/api/data/b/getcharacter?account={accountName}&name={characterName}&overview=necropolis&type=exp";
                
                // Query character from poeninja
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    if (response.StatusCode == HttpStatusCode.NotFound) continue;
                    
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();

                    if (!responseBody.Contains("Timeless Jewel")) continue;

                    // Create file
                    filePath = Path.Join(path, "character_data", $"char_{counter}.txt");
                    var file = File.Create(filePath);
                    file.Close();

                    // Otherwise, content found. Save it to the file
                    using (var writer = new StreamWriter(filePath))
                    {
                        writer.WriteLine(responseBody);
                    }

                    counter++;
                    Thread.Sleep(1000); // Sleep for 1 seconds
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine(e.Message);
                }
                
            }
        }
    }

    public static async Task<List<TimelessJewel>> FindCommonJewels()
    {
        var path = Utility.GetResourcesFilePath();
        var directoryPath = Path.Join(path, "character_data");
        
        var pattern = @"Timeless Jewel.+implicitMods.+explicitMods.+([^0-9]+([0-9]+).+ ([^\\]+))\\nP";
        
        var regex = new Regex(pattern, RegexOptions.Multiline);

        var jewels = new List<TimelessJewel>();

        // Get all files in the directory
        string[] files = Directory.GetFiles(directoryPath);

        // Iterate through each file
        foreach (string file in files)
        {
            // Perform an action on each file
            Console.WriteLine($"File: {file}");

            // Example: Read the content of the file
            var content = File.ReadAllText(file);

            var accountInfoString = content.Substring(0, content.IndexOf("defensiveStats"));
            var poeninjaString = GetNinjaUrl(accountInfoString);
            
            content = content.Substring(content.IndexOf("Timeless Jewel"));
            content = content.Substring(0, content.IndexOf("Historic"));
            
            var match = regex.Match(content);

            if (match.Success)
            {
                string group2 = match.Groups[2].Value;
                string group3 = match.Groups[3].Value;
                
                var type = TimelessJewel.parseConqueredByForType(group3);

                var jewel = new TimelessJewel("", "", type, group3, int.Parse(group2));

                var index = jewels.IndexOf(jewel);
                if (index != -1)
                {
                    jewels[index].Occurrences++;
                    jewels[index].poeninjaLinks.Add(poeninjaString);

                    if (jewels[index].ConqueredBy != group3) jewels[index].AllConqueredByTheSame = false;
                }
                else
                {
                    jewels.Add(jewel);
                    jewel.poeninjaLinks.Add(poeninjaString);
                    jewel.Occurrences = 1;
                }
            }
            else
            {
                Console.WriteLine("No match found.");
            }
        }

        var builder = new StringBuilder();
        for (var i = 0; i < jewels.Count; i++)
        {
            if (jewels[i].Occurrences >= 3)
            {
                builder.Append(jewels[i].ConqueredBy).Append(" ").Append(jewels[i].Number).Append(" ").Append(jewels[i].Occurrences);
                builder.Append(" ").Append(jewels[i].AllConqueredByTheSame);
                foreach (var link in jewels[i].poeninjaLinks)
                {
                    builder.Append(" ").Append(link);
                }

                builder.Append("\n");
            }
        }
        
        // Create file
        var filePath = Path.Join(path, "popularJewels.txt");
        var file2 = File.Create(filePath);
        file2.Close();

        // Otherwise, content found. Save it to the file
        using (var writer = new StreamWriter(filePath))
        {
            writer.WriteLine(builder.ToString());
        }
        

        return jewels;
    }

    private static string GetNinjaUrl(string json)
    {
        // Find the position of each key
        int accountIndex = json.IndexOf("\"account\":\"") + "\"account\":\"".Length;
        int nameIndex = json.IndexOf("\"name\":\"") + "\"name\":\"".Length;
        int leagueIndex = json.IndexOf("\"league\":\"") + "\"league\":\"".Length;

        // Extract the values using substring
        string account = json.Substring(accountIndex, json.IndexOf("\"", accountIndex) - accountIndex);
        string name = json.Substring(nameIndex, json.IndexOf("\"", nameIndex) - nameIndex);
        string league = json.Substring(leagueIndex, json.IndexOf("\"", leagueIndex) - leagueIndex);

        // Construct the URL
        string url = $"https://poe.ninja/builds/{league.ToLower()}/character/{account}/{name}";
        return url;
    }

    public static List<TimelessJewel> ParseSavedFile()
    {
        var path = Utility.GetResourcesFilePath();
        var filePath = Path.Join(path, "popularJewels.txt");
        var text = File.ReadAllText(filePath);

        var list = new List<TimelessJewel>();

        var jewels = text.Split("\n");
        foreach (var jewel in jewels)
        {
            try
            {
                var stats = jewel.Split(" ");
                var type = TimelessJewel.parseConqueredByForType(stats[0]);
                var timeless = new TimelessJewel("", "", type, stats[0], int.Parse(stats[1]));
                timeless.Occurrences = int.Parse(stats[2]);
                timeless.AllConqueredByTheSame = bool.Parse(stats[3]);

                for (var i = 4; i < stats.Length; i++)
                {
                    timeless.poeninjaLinks.Add(stats[i]);
                }

                list.Add(timeless);
            }
            catch (IndexOutOfRangeException e)
            {
                continue;
            }
        }

        return list;
    }
}