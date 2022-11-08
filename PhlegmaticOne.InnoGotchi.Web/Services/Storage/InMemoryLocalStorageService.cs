namespace PhlegmaticOne.InnoGotchi.Web.Services.Storage;

public class InMemoryLocalStorageService : ILocalStorageService
{
    private readonly Dictionary<string, object> _storage = new();
    public void SetValue<T>(string key, T value) => _storage.TryAdd(key, value);
    public T? GetValue<T>(string key) => _storage.TryGetValue(key, out var result) ? (T)result : default;
}