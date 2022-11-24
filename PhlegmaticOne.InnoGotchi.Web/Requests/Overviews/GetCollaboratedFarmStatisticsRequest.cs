using PhlegmaticOne.InnoGotchi.Shared.FarmStatistics;
using PhlegmaticOne.ServerRequesting.Models;

namespace PhlegmaticOne.InnoGotchi.Web.Requests.Overviews;

public class GetCollaboratedFarmStatisticsRequest : EmptyClientGetRequest<IList<PreviewFarmStatisticsDto>> { }