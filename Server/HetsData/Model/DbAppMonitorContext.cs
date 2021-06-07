using Microsoft.EntityFrameworkCore;

namespace HetsData.Model
{
    public partial class DbAppMonitorContext : DbAppContext
    {
        public DbAppMonitorContext(DbContextOptions<DbAppContext> options)
            : base(options)
        {
        }
    }
}
