using FluentAssertions;
using Moq;
using PhlegmaticOne.InnoGotchi.Domain.Exceptions;
using PhlegmaticOne.InnoGotchi.Domain.Models.Enums;
using PhlegmaticOne.InnoGotchi.Domain.Services;
using PhlegmaticOne.InnoGotchi.Services.Providers.Writable;
using PhlegmaticOne.InnoGotchi.Services.Tests.Infrastructure.Mocks;
using PhlegmaticOne.InnoGotchi.Shared.Components;
using PhlegmaticOne.InnoGotchi.Shared.Constructor;
using PhlegmaticOne.InnoGotchi.Shared.ErrorMessages;

namespace PhlegmaticOne.InnoGotchi.Services.Tests.Providers.Writable;

public class WritableInnoGotchiProviderTests
{
    private readonly UnitOfWorkMock _data;
    private readonly WritableInnoGotchiProvider _sut;
    private static readonly DateTime Now = DateTime.MinValue;
    public WritableInnoGotchiProviderTests()
    {
        _data = UnitOfWorkMock.Create();
        var timeService = new Mock<ITimeService>();
        timeService.Setup(x => x.Now()).Returns(Now);
        _sut = new WritableInnoGotchiProvider(_data.UnitOfWork, timeService.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowExceptionBecauseOfIncorrectComponent_Test()
    {
        var profileId = _data.ThatHasFarm.Id;

        var createInnoGotchiDto = new CreateInnoGotchiDto
        {
            Components = new List<InnoGotchiModelComponentDto>()
            {
                new()
                {
                    Name = Guid.NewGuid().ToString(),
                    ImageUrl = Guid.NewGuid().ToString()
                }
            },
            Name = "Name"
        };

        await _sut.Invoking(x => x.CreateAsync(profileId, createInnoGotchiDto))
            .Should()
            .ThrowAsync<DomainException>()
            .WithMessage(AppErrorMessages.UnknownComponentMessage);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowExceptionBecauseProfileDoesNotHaveFarm_Test()
    {
        var profileId = _data.ThatHasNoFarm.Id;
        var component = _data.AlivePet.Components.First().InnoGotchiComponent;

        var createInnoGotchiDto = new CreateInnoGotchiDto
        {
            Components = new List<InnoGotchiModelComponentDto>
            {
                new()
                {
                    Name = component.Name,
                    ImageUrl = component.ImageUrl
                }
            },
            Name = "Name"
        };

        await _sut.Invoking(x => x.CreateAsync(profileId, createInnoGotchiDto))
            .Should()
            .ThrowAsync<DomainException>()
            .WithMessage(AppErrorMessages.FarmDoesNotExistMessage);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnCreatedPet_Test()
    {
        var profileId = _data.ThatHasFarm.Id;
        var component = _data.AlivePet.Components.First().InnoGotchiComponent;
        var createInnoGotchiDto = new CreateInnoGotchiDto
        {
            Components = new List<InnoGotchiModelComponentDto>
            {
                new()
                {
                    Name = component.Name,
                    ImageUrl = component.ImageUrl
                }
            },
            Name = "Name"
        };

        var pet = await _sut.CreateAsync(profileId, createInnoGotchiDto);

        pet.Components.Should().HaveCount(1).And.ContainSingle(x =>
                x.InnoGotchiComponent.Name == component.Name && x.InnoGotchiComponent.ImageUrl == component.ImageUrl);
        pet.Age.Should().Be(0);
        pet.AgeUpdatedAt.Should().Be(Now);
        pet.DeadSince.Should().Be(DateTime.MinValue);
        pet.HappinessDaysCount.Should().Be(0);
        pet.HungerLevel.Should().Be(HungerLevel.Normal);
        pet.IsDead.Should().Be(false);
        pet.LastDrinkTime.Should().Be(Now);
        pet.LastFeedTime.Should().Be(Now);
        pet.Name.Should().Be("Name");
        pet.LiveSince.Should().Be(Now);
        pet.ThirstyLevel.Should().Be(ThirstyLevel.Normal);
    }

    [Fact]
    public async Task DrinkAsync_ShouldUpdateAndReturnPet_Test()
    {
        var petId = _data.AlivePet.Id;

        var updated = await _sut.DrinkAsync(petId);

        updated.LastDrinkTime.Should().Be(Now);
        updated.ThirstyLevel.Should().Be(ThirstyLevel.Full);
    }

    [Fact]
    public async Task FeedAsync_ShouldUpdateAndReturnPet_Test()
    {
        var petId = _data.AlivePet.Id;

        var updated = await _sut.FeedAsync(petId);

        updated.LastFeedTime.Should().Be(Now);
        updated.HungerLevel.Should().Be(HungerLevel.Full);
    }
}