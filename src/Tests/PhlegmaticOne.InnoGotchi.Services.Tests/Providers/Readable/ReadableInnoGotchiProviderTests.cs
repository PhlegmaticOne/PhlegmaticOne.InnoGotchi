using FluentAssertions;
using PhlegmaticOne.InnoGotchi.Services.Providers.Readable;
using PhlegmaticOne.InnoGotchi.Services.Tests.Infrastructure.Mocks;

namespace PhlegmaticOne.InnoGotchi.Services.Tests.Providers.Readable;

public class ReadableInnoGotchiProviderTests
{
    private readonly UnitOfWorkMock _data;
    public ReadableInnoGotchiProviderTests() => _data = UnitOfWorkMock.Create();

    [Fact]
    public async Task GetDetailedAsync_ShouldReturnPet_Test()
    {
        var petId = _data.AlivePet.Id;
        var sut = new ReadableInnoGotchiProvider(_data.UnitOfWork);

        var pet = await sut.GetDetailedAsync(petId);

        pet.Should().NotBeNull();
    }

    [Fact]
    public async Task GetDetailedAsync_ShouldReturnNull_Test()
    {
        var petId = Guid.Empty;
        var sut = new ReadableInnoGotchiProvider(_data.UnitOfWork);

        var pet = await sut.GetDetailedAsync(petId);

        pet.Should().BeNull();
    }
}