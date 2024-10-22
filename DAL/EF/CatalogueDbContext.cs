using ArticleManagement.BL.Domain;
using Microsoft.EntityFrameworkCore;

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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // Configurations if no options are provided via the constructor
            optionsBuilder.UseSqlite("Data Source=AppDatabase.sqlite");
        }
        //...
    }
}