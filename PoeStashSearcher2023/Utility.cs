namespace PoeStashSearcher2023;

public class Utility
{
    public static string GetResourcesFilePath()
    {
        var workingDirectory = Environment.CurrentDirectory;
        var projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
        var path = Path.Join(projectDirectory, "resources");
        return path;
    }
}