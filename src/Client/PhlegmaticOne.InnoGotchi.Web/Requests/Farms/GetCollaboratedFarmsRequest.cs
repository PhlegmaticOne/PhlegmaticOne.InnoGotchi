﻿using PhlegmaticOne.InnoGotchi.Shared.Farms;
using PhlegmaticOne.ServerRequesting.Models.Requests;

namespace PhlegmaticOne.InnoGotchi.Web.Requests.Farms;

public class GetCollaboratedFarmsRequest : EmptyClientGetRequest<IList<PreviewFarmDto>> { }