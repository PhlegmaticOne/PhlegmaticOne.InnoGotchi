using Microsoft.EntityFrameworkCore;
using PhlegmaticOne.InnoGotchi.Data.EntityFramework.Context;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Domain.Models.Enums;
using PhlegmaticOne.UnitOfWork.Implementation;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Tests.Mocks;

public class UnitOfWorkMock
{
    public static UnitOfWorkData Create()
    {
        var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        var dbContext = new ApplicationDbContext(contextOptions);
        var unitOfWork = new DbContextUnitOfWork(dbContext);
        var profiles = GetUsers();
        var collaboration = GetCollaboration(profiles);
        var pets = GetPets(profiles);
        dbContext.Set<UserProfile>().AddRange(profiles);
        dbContext.Set<Collaboration>().Add(collaboration);
        dbContext.Set<InnoGotchiModel>().AddRange(pets);
        dbContext.SaveChanges();

        return new UnitOfWorkData(profiles, pets[0], pets[1], collaboration, unitOfWork);
    }

    private static Collaboration GetCollaboration(List<UserProfile> profiles) =>
        new()
        {
            Collaborator = profiles[1],
            Farm = profiles.First().Farm!
        };
    private static List<InnoGotchiModel> GetPets(List<UserProfile> profiles)
    {
        var farm = profiles.First().Farm!;
        var now = DateTime.Now;
        var components = new List<InnoGotchiModelComponent>()
        {
            new()
            {
                InnoGotchiComponent = new()
                {
                    Name = "Bodies",
                    ImageUrl = "BodyUrl"
                },
            },
            new()
            {
                InnoGotchiComponent = new()
                {
                    Name = "Noses",
                    ImageUrl = "NosesUrl"
                }
            }
        };

        return new()
        {
            new()
            {
                Age = 0,
                AgeUpdatedAt = now,
                Components = components,
                DeadSince = DateTime.MinValue,
                Farm = farm,
                HappinessDaysCount = 0,
                HungerLevel = HungerLevel.Normal,
                IsDead = false,
                LastDrinkTime = now,
                LastFeedTime = now,
                LiveSince = now,
                Name = "pet",
                ThirstyLevel = ThirstyLevel.Normal
            },
            new()
            {
                Age = 10,
                AgeUpdatedAt = now,
                Components = components,
                DeadSince = now,
                Farm = farm,
                HappinessDaysCount = 1,
                HungerLevel = HungerLevel.Dead,
                IsDead = true,
                LastDrinkTime = now,
                LastFeedTime = now,
                LiveSince = now,
                Name = "pet1",
                ThirstyLevel = ThirstyLevel.Dead
            },
        };
    }
    private static List<UserProfile> GetUsers()
    {
        return new List<UserProfile>
        {
            new()
            {
                User = new User
                {
                    Email = "test@gmail.com",
                    Password = "qwerty_1234"
                },
                Avatar = new Avatar(),
                FirstName = "Firstname",
                LastName = "Secondname",
                JoinDate = DateTime.MinValue,
                Farm = new Farm
                {
                    FarmStatistics = new FarmStatistics(),
                    Name = "my farm"
                }
            },

            new()
            {
                User = new User
                {
                    Email = "new@gmail.com",
                    Password = "qwerty_1234"
                },
                Avatar = new Avatar(),
                FirstName = "Name",
                LastName = "Lastname",
                JoinDate = DateTime.MinValue,
                Farm = new Farm
                {
                    FarmStatistics = new FarmStatistics(),
                    Name = "new farm"
                }
            },
            new()
            {
                User = new User
                {
                    Email = "t@gmail.com",
                    Password = "qwerty_1234"
                },
                Avatar = new Avatar(),
                FirstName = "AOisfnaf",
                LastName = "Asmalsf",
                JoinDate = DateTime.MinValue,
                Farm = new Farm
                {
                    FarmStatistics = new FarmStatistics(),
                    Name = "nn farm"
                }
            },
            new()
            {
                User = new User
                {
                    Email = "te@gmail.com",
                    Password = "qwerty_1234"
                },
                Avatar = new Avatar(),
                FirstName = "ASLKFN",
                LastName = "VSJRGo",
                JoinDate = DateTime.MinValue,
                Farm = new Farm
                {
                    FarmStatistics = new FarmStatistics(),
                    Name = "xx farm"
                }
            },
            new()
            {
                User = new User
                {
                    Email = "tes@gmail.com",
                    Password = "qwerty_1234"
                },
                Avatar = new Avatar(),
                FirstName = "OEITMW",
                LastName = "CMAKFE",
                JoinDate = DateTime.MinValue
            }
        };
    }
}

public class UnitOfWorkData
{
    public UnitOfWorkData(List<UserProfile> entities,
        InnoGotchiModel innoGotchiModel,
        InnoGotchiModel deadPet,
        Collaboration createdCollaboration,
        IUnitOfWork unitOfWork)
    {
        Profiles = entities;
        AlivePet = innoGotchiModel;
        DeadPet = deadPet;
        CreatedCollaboration = createdCollaboration;
        UnitOfWork = unitOfWork;
    }

    public IUnitOfWork UnitOfWork { get; }
    public List<UserProfile> Profiles { get; }
    public InnoGotchiModel AlivePet { get; }
    public InnoGotchiModel DeadPet { get; }
    public UserProfile ThatHasNoFarm => Profiles.Last();
    public UserProfile ThatHasFarm => Profiles.First();
    public string ReservedFarmName => ThatHasFarm.Farm!.Name;
    public Collaboration CreatedCollaboration { get; }
}