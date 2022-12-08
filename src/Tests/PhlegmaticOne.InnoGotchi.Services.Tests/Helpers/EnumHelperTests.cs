using FluentAssertions;
using PhlegmaticOne.InnoGotchi.Services.Infrastructure.Helpers;

namespace PhlegmaticOne.InnoGotchi.Services.Tests.Helpers;

public class EnumHelperTests
{
    private enum TestEnum
    {
        First,
        Second,
        Third
    }

    [Fact]
    public void EnumValueOrMax_ShouldReturnPassedValueBecauseItIsLessThanEnumMaxValue_Test()
    {
        var expected = (int)TestEnum.Second;
        var actual = EnumHelper.EnumValueOrMax<TestEnum>(expected);
        actual.Should().Be(TestEnum.Second);
    }

    [Fact]
    public void EnumValueOrMax_ShouldReturnMaxValueBecausePassedValueIsGreaterThanMaxValue_Test()
    {
        var expected = 10;
        var actual = EnumHelper.EnumValueOrMax<TestEnum>(expected);

        actual.Should().Be(TestEnum.Third);
    }
}