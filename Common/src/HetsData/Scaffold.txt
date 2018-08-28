Generate / Update Model:
************************

Scaffold-DbContext "Host=localhost;Username=user6DA;Password=IhUFdcC0wGJeIMDJ;Database=hets" Npgsql.EntityFrameworkCore.PostgreSQL -OutputDir Model -Force -Project "HetsData" -Verbose -Context "DbAppContext"


********************************************
Step 2: Update Context
********************************************
After generating the model need to update the DbAppContext class to manage the connection string.

Add new constructor:

private readonly string _connectionString;

public DbAppContext(string connectionString)
{
    _connectionString = connectionString;
}



protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    if (!optionsBuilder.IsConfigured)
    {
        optionsBuilder.UseNpgsql(_connectionString);
    }
}