using Microsoft.EntityFrameworkCore;
using PhlegmaticOne.InnoGotchi.Data.EntityFramework.Context;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Services.Tests.Infrastructure.Data;
using PhlegmaticOne.UnitOfWork.Implementation;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Tests.Infrastructure.Mocks;

public class UnitOfWorkMock
{
    public static UnitOfWorkMock Create()
    {
        var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        var dbContext = new ApplicationDbContext(contextOptions);
        var unitOfWork = new DbContextUnitOfWork(dbContext);
        var data = new DomainData();
        var profiles = data.GetProfiles();
        var collaboration = data.GetCollaboration();
        var pets = data.GetPetsForFirstProfile();
        dbContext.Set<UserProfile>().AddRange(profiles);
        dbContext.Set<Collaboration>().Add(collaboration);
        dbContext.Set<InnoGotchiModel>().AddRange(pets);
        dbContext.SaveChanges();

        return new UnitOfWorkMock(profiles, pets[0], pets[1], collaboration, data, unitOfWork);
    }

    private UnitOfWorkMock(List<UserProfile> profiles,
        InnoGotchiModel alivePet,
        InnoGotchiModel deadPet,
        Collaboration createdCollaboration,
        DomainData data,
        IUnitOfWork unitOfWork)
    {
        Profiles = profiles;
        AlivePet = alivePet;
        DeadPet = deadPet;
        CreatedCollaboration = createdCollaboration;
        Data = data;
        UnitOfWork = unitOfWork;
    }
    public DomainData Data { get; }
    public IUnitOfWork UnitOfWork { get; }
    public List<UserProfile> Profiles { get; }
    public InnoGotchiModel AlivePet { get; }
    public InnoGotchiModel DeadPet { get; }
    public UserProfile ThatHasNoFarm => Profiles.Last();
    public UserProfile ThatHasFarm => Profiles.First();
    public string ReservedFarmName => ThatHasFarm.Farm!.Name;
    public Collaboration CreatedCollaboration { get; }
}