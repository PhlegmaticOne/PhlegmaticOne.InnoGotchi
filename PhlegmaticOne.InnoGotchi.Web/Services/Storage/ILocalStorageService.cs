namespace PhlegmaticOne.InnoGotchi.Web.Services.Storage;

public interface ILocalStorageService
{
    void SetValue<T>(string key, T value);
    T? GetValue<T>(string key);
}