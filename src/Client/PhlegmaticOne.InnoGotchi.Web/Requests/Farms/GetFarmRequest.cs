﻿using PhlegmaticOne.InnoGotchi.Shared.Farms;
using PhlegmaticOne.ServerRequesting.Models;
using PhlegmaticOne.ServerRequesting.Models.Requests;

namespace PhlegmaticOne.InnoGotchi.Web.Requests.Farms;

public class GetFarmRequest : ClientGetRequest<Guid, DetailedFarmDto>
{
    public GetFarmRequest(Guid requestData) : base(requestData)
    {
    }

    public override string BuildQueryString()
    {
        return WithOneQueryParameter(new GetRequestQueryParameter("farmId", RequestData));
    }
}