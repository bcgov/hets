using HetsData.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace HetsCommon.Tests;

public class PostgreSqlProviderTests
{
    [Fact]
    public void ApiPackageGraphCanTranslateUserLookup()
    {
        var options = new DbContextOptionsBuilder<DbAppContext>()
            .UseNpgsql("Host=127.0.0.1;Database=unused;Username=unused;Password=unused")
            .Options;

        using var context = new DbAppContext(options, NullLogger<DbAppContext>.Instance);

        var sql = context.HetUsers
            .AsNoTracking()
            .Where(user => user.SmUserId == "smoke")
            .ToQueryString();

        Assert.Contains("HET_USER", sql);
    }
}
