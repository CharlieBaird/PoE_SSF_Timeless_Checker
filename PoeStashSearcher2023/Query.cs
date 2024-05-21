namespace PoeStashSearcher2023;

public class Query
{
    private HttpClient Client;

    public Query(string POESESSID)
    {
        Client = new HttpClient();
        Client.DefaultRequestHeaders.Add("Cookie", "POESESSID=" + POESESSID);
    }

    public async Task<string?> Get(string endpoint)
    {
        if (string.IsNullOrWhiteSpace(endpoint)) 
            return null;
        
        var response = await Client.GetAsync(endpoint);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }

        Console.WriteLine("Error occurred in the get call:");
        Console.WriteLine(response.ReasonPhrase);

        return null;
    }
}