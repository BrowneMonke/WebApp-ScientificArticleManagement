using System.Diagnostics;
using ArticleManagement.BL.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ArticleManagement.DAL.EF;

public class ArticleDbContext : IdentityDbContext
{
    public DbSet<Scientist> Scientists { get; set; }
    public DbSet<ScientificArticle> Articles { get; set; }
    public DbSet<ArticleScientistLink> ArticleScientistLinks { get; set; }
    public DbSet<ScienceJournal> Journals { get; set; }
    
    public ArticleDbContext(DbContextOptions options) : base(options)
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
            optionsBuilder.UseSqlite("Data Source=../../../../ArticleDatabase.db");
        }

        // lazy-loading => nav-props must be made virtual
        optionsBuilder.UseLazyLoadingProxies(false); // use 'false' as argument to disable!
        
        optionsBuilder.LogTo(logMsg => Debug.WriteLine(logMsg), LogLevel.Information);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ScientificArticle>()
            .HasOne(art => art.Journal)
            .WithMany(j => j.Articles)
            .IsRequired(false); // 0..1 ipv 1
        /*
         modelBuilder.Entity<ScienceJournal>() // duplicate code of above ↑
            .HasMany(j => j.Articles)
            .WithOne(art => art.Journal)
            .IsRequired(false);
        */

        modelBuilder.Entity<ArticleScientistLink>().Property<int>("fkArticleId");
        modelBuilder.Entity<ArticleScientistLink>().Property<int>("fkScientistId"); // shadow properties
        
        modelBuilder.Entity<ArticleScientistLink>()
            .HasOne(artSc => artSc.Article)
            .WithMany(art => art.AuthorLinks)
            .HasForeignKey("fkArticleId")
            .IsRequired();
        
        modelBuilder.Entity<ArticleScientistLink>()
            .HasOne(artSc => artSc.Scientist)
            .WithMany(sc => sc.ArticleLinks)
            .HasForeignKey("fkScientistId")
            .IsRequired();

        modelBuilder.Entity<ArticleScientistLink>()
            .HasKey("fkArticleId", "fkScientistId");
    }
    
}