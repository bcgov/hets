using HetsCommon;
using Xunit;

namespace HetsCommon.Tests;

public class MinistryNameHelperTests
{
    [Theory]
    [InlineData("Ministry of Transportation and Infrastructure|South Peace")]
    [InlineData("Ministry of Transportation  and Infrastructure|North Peace")]
    [InlineData("Ministry of Transportation amp; Infrastructure|West Kootenay")]
    [InlineData("Ministry of Transportation &amp; Infrastructure|West Kootenay")]
    [InlineData("Ministry of Transportation & Infrastructure|West Kootenay")]
    public void UseCurrentNameReplacesFormerNameVariants(string address)
    {
        var result = MinistryNameHelper.UseCurrentName(address);

        Assert.StartsWith("Ministry of Transportation and Transit|", result);
        Assert.DoesNotContain("Infrastructure", result);
    }

    [Fact]
    public void UseCurrentNameLeavesCurrentNameUnchanged()
    {
        const string address = "Ministry of Transportation and Transit|North Peace";

        Assert.Equal(address, MinistryNameHelper.UseCurrentName(address));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void UseCurrentNameReturnsEmptyStringForMissingValue(string? address)
    {
        Assert.Equal("", MinistryNameHelper.UseCurrentName(address));
    }
}
