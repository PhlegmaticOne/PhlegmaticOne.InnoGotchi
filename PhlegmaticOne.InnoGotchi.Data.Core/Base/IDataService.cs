using PhlegmaticOne.InnoGotchi.Data.Models.Base;

namespace PhlegmaticOne.InnoGotchi.Data.Core.Base;

public interface IDataService
{
    IDataRepository<TEntity> GetDataRepository<TEntity>() where TEntity : ModelBase;
}