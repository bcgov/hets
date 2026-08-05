using HetsCommon;
using Xunit;

namespace HetsCommon.Tests;

public class DateUtilsTests
{
    [Theory]
    [InlineData(DateTimeKind.Utc)]
    [InlineData(DateTimeKind.Local)]
    [InlineData(DateTimeKind.Unspecified)]
    public void FormatDateOnlyPreservesCalendarDate(DateTimeKind kind)
    {
        var selectedStartDate = new DateTime(2026, 4, 2, 0, 0, 0, kind);

        var result = DateUtils.FormatDateOnly(selectedStartDate);

        Assert.Equal("2026-APR-02", result);
    }

    [Fact]
    public void FormatDateOnlyReturnsEmptyStringForNull()
    {
        Assert.Equal("", DateUtils.FormatDateOnly(null));
    }
}
