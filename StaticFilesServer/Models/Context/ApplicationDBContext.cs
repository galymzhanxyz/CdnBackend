using Microsoft.EntityFrameworkCore;

namespace StaticFilesServer.Models.Context
{
    public class ApplicationDBContext : DbContext
    {
        public DbSet<ClientBrowsers> ClientBrowser { get; set; }
        public DbSet<Sources> Source { get; set; }
        public DbSet<SourceStatistics> SourceStatistic { get; set; }
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
    }
}
