using System.Linq.Expressions;
using PhlegmaticOne.InnoGotchi.Domain.Models;

namespace PhlegmaticOne.InnoGotchi.Services.Infrastructure.Helpers;

public static class InnoGotchiSortingExpressions
{
    public static Dictionary<int, Expression<Func<InnoGotchiModel, object>>> GetSortingExpressions()
    {
        return new Dictionary<int, Expression<Func<InnoGotchiModel, object>>>
        {
            { 0, model => model.HappinessDaysCount },
            { 1, model => model.Age },
            { 2, model => model.HungerLevel },
            { 3, model => model.ThirstyLevel },
            { 4, model => model.Name },
            { 5, model => model.Farm.Name }
        };
    }
}