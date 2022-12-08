using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Domain.Models.Enums;

namespace PhlegmaticOne.InnoGotchi.Services.Tests.Infrastructure.Data;

public class DomainData
{
    private readonly List<UserProfile> _profiles = new()
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

    public Collaboration GetCollaboration()
    {
        return new()
        {
            Collaborator = _profiles[1],
            Farm = _profiles[0].Farm!
        };
    }

    public List<InnoGotchiModel> GetPetsForFirstProfile()
    {
        var farm = _profiles[0].Farm!;
        var now = DateTime.Now;
        var components = new List<InnoGotchiModelComponent>
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

    public List<UserProfile> GetProfiles() => _profiles;
}