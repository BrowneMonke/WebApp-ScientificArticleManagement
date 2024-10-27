using System.Diagnostics;
using ArticleManagement.BL.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ArticleManagement.DAL.EF;

public class CatalogueDbContext : DbContext
{
    public DbSet<Scientist> Scientists { get; set; }
    public DbSet<ScientificArticle> Articles { get; set; }
    public DbSet<ScienceJournal> Journals { get; set; }
    
    public CatalogueDbContext(DbContextOptions options) : base(options)
    {
        //...
    }

    public bool CreateDatabase(bool doDropDatabase)
    {
        if (doDropDatabase) Database.EnsureDeleted();
        return Database.EnsureCreated(); // Code First trigger
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // Configurations if no options are provided via the constructor (fall back to...)
            optionsBuilder.UseSqlite("Data Source=CatalogueDatabase.sqlite");
        }
        //...
        optionsBuilder.LogTo(logMsg => Debug.WriteLine(logMsg), LogLevel.Information);
    }
}