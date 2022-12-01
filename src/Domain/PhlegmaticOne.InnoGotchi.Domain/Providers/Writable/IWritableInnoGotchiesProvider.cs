﻿using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Shared.Constructor;

namespace PhlegmaticOne.InnoGotchi.Domain.Providers.Writable;

public interface IWritableInnoGotchiesProvider
{
    Task<InnoGotchiModel> CreateAsync(Guid profileId, CreateInnoGotchiDto createInnoGotchiDto,
        CancellationToken cancellationToken = new());
    Task DrinkAsync(Guid petId, CancellationToken cancellationToken = new());
    Task FeedAsync(Guid petId, CancellationToken cancellationToken = new());
}