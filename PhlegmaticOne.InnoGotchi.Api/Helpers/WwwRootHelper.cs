namespace PhlegmaticOne.InnoGotchi.Api.Helpers;

internal static class WwwRootHelper
{
    public static Dictionary<string, List<string>> GetComponents(IWebHostEnvironment webHostEnvironment)
    {
        const string initialDirectory = "Resources";
        var initialPath = Path.Combine(webHostEnvironment.WebRootPath, initialDirectory);
        var componentDirectories = new DirectoryInfo(initialPath);
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
}