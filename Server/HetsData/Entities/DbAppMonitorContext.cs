using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HetsData.Entities
{
    public partial class DbAppMonitorContext : DbAppContext
    {
        public DbAppMonitorContext(DbContextOptions<DbAppContext> options, ILogger<DbAppMonitorContext> logger)
            : base(options, logger)
        {
        }
    }
}
