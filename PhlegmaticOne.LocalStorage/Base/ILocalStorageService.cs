namespace PhlegmaticOne.LocalStorage.Base;

public interface ILocalStorageService
{
    void SetValue<T>(string key, T value);
    bool ContainsKey(string key);
    T? GetValue<T>(string key);
}