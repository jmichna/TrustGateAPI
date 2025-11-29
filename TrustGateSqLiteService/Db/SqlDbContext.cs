using Microsoft.EntityFrameworkCore;
using TrustGateCore.Models;

namespace TrustGateSqlLiteService.Db;

public class SqlDbContext : DbContext
{
    public SqlDbContext(DbContextOptions<SqlDbContext> options) : base(options) { }


    //protected override void OnConfiguring(DbContextOptionsBuilder options)
    //{
    //    if (!options.IsConfigured)
    //        options.UseSqlite("Data Source=C:\\Users\\jmich\\OneDrive\\Pulpit\\backup\\test\\trustgate.db");
    //}

    public DbSet<User> Users => Set<User>();
    public DbSet<Authorization> Authorizations => Set<Authorization>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Admin> Admins => Set<Admin>();
    public DbSet<ApiEndpoint> ApiEndpoints => Set<ApiEndpoint>();
    public DbSet<ApiToken> ApiTokens => Set<ApiToken>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SqlDbContext).Assembly);
    }
}