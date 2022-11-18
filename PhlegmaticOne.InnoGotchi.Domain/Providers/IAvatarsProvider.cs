﻿using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Domain.Providers;

public interface IAvatarsProvider
{
    Task<OperationResult<Avatar>> GetAvatarAsync(Guid profileId);
}