using PhlegmaticOne.InnoGotchi.Shared.FarmStatistics;
using PhlegmaticOne.ServerRequesting.Models.Requests;

namespace PhlegmaticOne.InnoGotchi.Web.Requests.Overviews;

public class GetCollaboratedFarmStatisticsRequest : EmptyClientGetRequest<IList<PreviewFarmStatisticsDto>>
{
}