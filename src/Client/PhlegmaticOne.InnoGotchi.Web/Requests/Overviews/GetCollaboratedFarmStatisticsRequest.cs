using PhlegmaticOne.InnoGotchi.Shared.Statistics;
using PhlegmaticOne.ServerRequesting.Models.Requests;

namespace PhlegmaticOne.InnoGotchi.Web.Requests.Overviews;

public class GetCollaboratedFarmStatisticsRequest : EmptyClientGetRequest<IList<PreviewFarmStatisticsDto>>
{
}