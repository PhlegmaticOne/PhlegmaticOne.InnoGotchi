using PhlegmaticOne.InnoGotchi.Shared.FarmStatistics;
using PhlegmaticOne.ServerRequesting.Models;

namespace PhlegmaticOne.InnoGotchi.Web.ClientRequests;

public class GetCollaboratorsFarmStatisticsGetRequest : EmptyClientGetRequest<IList<PreviewFarmStatisticsDto>> { }