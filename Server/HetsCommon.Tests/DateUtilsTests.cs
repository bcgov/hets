using System;
using HetsCommon;
using Xunit;

namespace HetsCommon.Tests;

public class DateUtilsTests
{
    [Theory]
    [InlineData(2026, 3, 9, 0, 0, 0, 2026, 3, 8, 17, 0, 0)]
    [InlineData(2026, 11, 1, 9, 0, 0, 2026, 11, 1, 2, 0, 0)]
    [InlineData(2026, 12, 15, 12, 0, 0, 2026, 12, 15, 5, 0, 0)]
    public void ConvertUtcToPacificTime_UsesPermanentUtcMinusSevenAfterEffectiveDate(
        int utcYear, int utcMonth, int utcDay, int utcHour, int utcMinute, int utcSecond,
        int expectedYear, int expectedMonth, int expectedDay, int expectedHour, int expectedMinute, int expectedSecond)
    {
        DateTime utc = new(utcYear, utcMonth, utcDay, utcHour, utcMinute, utcSecond, DateTimeKind.Utc);

        DateTime result = DateUtils.ConvertUtcToPacificTime(utc);

        Assert.Equal(new DateTime(expectedYear, expectedMonth, expectedDay, expectedHour, expectedMinute, expectedSecond), result);
        Assert.Equal(DateTimeKind.Unspecified, result.Kind);
    }

    [Theory]
    [InlineData(2026, 3, 9, 0, 0, 0, 2026, 3, 9, 7, 0, 0)]
    [InlineData(2026, 11, 1, 2, 0, 0, 2026, 11, 1, 9, 0, 0)]
    [InlineData(2026, 12, 15, 5, 0, 0, 2026, 12, 15, 12, 0, 0)]
    public void ConvertPacificToUtcTime_UsesPermanentUtcMinusSevenAfterEffectiveDate(
        int localYear, int localMonth, int localDay, int localHour, int localMinute, int localSecond,
        int expectedYear, int expectedMonth, int expectedDay, int expectedHour, int expectedMinute, int expectedSecond)
    {
        DateTime local = new(localYear, localMonth, localDay, localHour, localMinute, localSecond, DateTimeKind.Unspecified);

        DateTime result = DateUtils.ConvertPacificToUtcTime(local);

        Assert.Equal(new DateTime(expectedYear, expectedMonth, expectedDay, expectedHour, expectedMinute, expectedSecond, DateTimeKind.Utc), result);
        Assert.Equal(DateTimeKind.Utc, result.Kind);
    }

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
