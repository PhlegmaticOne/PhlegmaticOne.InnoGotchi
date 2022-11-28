namespace PhlegmaticOne.InnoGotchi.Api.Infrastructure.Helpers;

internal static class WwwRootHelper
{
    public static Dictionary<string, List<string>> GetComponents(IWebHostEnvironment webHostEnvironment)
    {
        const string initialDirectory = "Resources";
        var componentDirectories = GetDirectoryInfo(webHostEnvironment, initialDirectory);
        var componentPaths = new Dictionary<string, List<string>>();

        foreach (var directory in componentDirectories.EnumerateDirectories())
        {
            var filePaths = directory
                .EnumerateFiles()
                .Select(file => initialDirectory + "/" + directory.Name + "/" + file.Name)
                .ToList();
            componentPaths.Add(directory.Name, filePaths);
        }

        return componentPaths;
    }

    private static DirectoryInfo GetDirectoryInfo(IWebHostEnvironment webHostEnvironment, string directoryName)
    {
        var initialPath = Path.Combine(webHostEnvironment.WebRootPath, directoryName);
        return new DirectoryInfo(initialPath);
    }
}