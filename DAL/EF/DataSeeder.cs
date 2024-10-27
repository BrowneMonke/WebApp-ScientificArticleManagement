using ArticleManagement.BL.Domain;

namespace ArticleManagement.DAL.EF;

public static class DataSeeder
{ 
    private static void SeedScientists(CatalogueDbContext catalogueDbContext)
    {
        Scientist walterLewin = new Scientist("Walter H. G. Lewin", "Physics", "MIT", new DateOnly(1936, 1, 29));
        Scientist janVanParadijs = new Scientist("Jan Van Paradijs", "Physics", "University Of Amsterdam", new DateOnly(1946, 6, 9));
        Scientist holgerPedersen = new Scientist("Holger Pedersen", "Physics", "Niels Bohr Institute", new DateOnly(1946, 11, 3));
        Scientist paulJoss = new Scientist("Paul C. Joss", "Physics", "MIT");
        Scientist charlesWarwick = new Scientist("Charles Warwick", "Neuroscience", "University of Pittsburgh");
        Scientist anelaChoy = new Scientist("Anela Choy", "Oceanography", "UC San Diego");
        Scientist robSherlock = new Scientist("Robert E. Sherlock", "Research", "MBARI", new DateOnly(1966, 11, 1));

        catalogueDbContext.Scientists.AddRange(walterLewin, janVanParadijs, holgerPedersen, paulJoss, charlesWarwick, anelaChoy, robSherlock);

        int id = 1;
        foreach (Scientist scientist in catalogueDbContext.Scientists)
        {
            scientist.ScientistId = id++;
        }

        catalogueDbContext.SaveChanges();
    }

    private static void SeedJournals(CatalogueDbContext catalogueDbContext)
    {
        ScienceJournal journalNature = new ScienceJournal("Nature");
        ScienceJournal journalScAdvances = new ScienceJournal("Science Advances", 15.00);

        catalogueDbContext.Journals.AddRange(journalNature, journalScAdvances);
        
        int id = 1;
        foreach (ScienceJournal journal in catalogueDbContext.Journals)
        {
            journal.JournalId = id++;
        }

        catalogueDbContext.SaveChanges();
    }

    private static void SeedArticles(CatalogueDbContext catalogueDbContext)
    {
        List<Scientist> authorsList = catalogueDbContext.Scientists.ToList();
        List<ScienceJournal> journalsList = catalogueDbContext.Journals.ToList();
        
        ScientificArticle articleOrbitalPeriodXRayBurster =
            new ScientificArticle("A four-hour orbital period of the X-ray burster 4U/MXB1636—53")
            {
                Authors = [authorsList[0], authorsList[1], authorsList[2]],
                DateOfPublication = new DateOnly(1981, 12, 31),
                NumberOfPages = 3,
                Category = ArticleCategory.Astrophysics,
                Journal = journalsList[0]
            };
        ScientificArticle articleXRayBurstSources =
            new ScientificArticle("X-ray burst sources")
            {
                Authors = [authorsList[0], authorsList[3]],
                DateOfPublication = new DateOnly(1977, 11, 17),
                NumberOfPages = 6,
                Category = ArticleCategory.Astrophysics,
                Journal = journalsList[0]
            };
      
        ScientificArticle articleKappa =
            new ScientificArticle("Kappa opioids inhibit spinal output neurons to suppress itch")
            {
                Authors = [authorsList[4]],
                DateOfPublication = new DateOnly(2024, 09, 25),
                NumberOfPages = 18,
                Category = ArticleCategory.Neuroscience,
                Journal = journalsList[1]
            };
        ScientificArticle articleLarvaceans =
            new ScientificArticle("From the surface to the seafloor: How giant larvaceans transport microplastics into the deep sea")
            {
                Authors = [authorsList[5], authorsList[6]],
                DateOfPublication = new DateOnly(2017, 8, 16),
                NumberOfPages = 5,
                Category = ArticleCategory.MarineEcology,
                Journal = journalsList[1]
            };

        catalogueDbContext.Articles.AddRange(articleOrbitalPeriodXRayBurster, articleXRayBurstSources, articleKappa, articleLarvaceans);
        
        int id = 1;
        foreach (ScientificArticle article in catalogueDbContext.Articles)
        {
            article.ArticleId = id++;
        }

        catalogueDbContext.SaveChanges();
    }

    public static void Seed(CatalogueDbContext catalogueDbContext)
    {
        SeedScientists(catalogueDbContext);
        SeedJournals(catalogueDbContext);
        SeedArticles(catalogueDbContext);
        catalogueDbContext.ChangeTracker.Clear();
    }
}