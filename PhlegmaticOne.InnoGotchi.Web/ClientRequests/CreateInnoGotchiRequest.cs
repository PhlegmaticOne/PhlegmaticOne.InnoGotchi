﻿using PhlegmaticOne.InnoGotchi.Shared.Constructor;
using PhlegmaticOne.InnoGotchi.Shared.InnoGotchies;
using PhlegmaticOne.ServerRequesting.Models;

namespace PhlegmaticOne.InnoGotchi.Web.ClientRequests;

public class CreateInnoGotchiRequest : ClientPostRequest<CreateInnoGotchiDto, InnoGotchiDto>
{
    public CreateInnoGotchiRequest(CreateInnoGotchiDto requestData) : base(requestData) { }
}