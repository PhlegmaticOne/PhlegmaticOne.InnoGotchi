﻿using PhlegmaticOne.LocalStorage.Base;

namespace PhlegmaticOne.LocalStorage;

public class InMemoryLocalStorageService : ILocalStorageService
{
    private readonly Dictionary<string, object> _storage = new();
    public void SetValue<T>(string key, T value)
    {
        if (_storage.ContainsKey(key))
        {
            _storage[key] = value!;
            return;
        }

        _storage.Add(key, value!);
    }

    public bool ContainsKey(string key) => _storage.ContainsKey(key);

    public T? GetValue<T>(string key) => _storage.TryGetValue(key, out var result) ? (T)result : default;
}