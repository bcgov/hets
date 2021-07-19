using Microsoft.EntityFrameworkCore;

namespace HetsData.Entities
{
    public partial class DbAppMonitorContext : DbAppContext
    {
        public DbAppMonitorContext(DbContextOptions<DbAppContext> options)
            : base(options)
        {
        }
    }
}
