/*using ArticleManagement.BL.Domain;

namespace ArticleManagement.DAL.EF;

public static class DataSeeder
{ 
    private static void SeedScientists(CatalogueDbContext catalogueDbContext)
    {
        Scientist walterLewin = new Scientist("Walter H. G. Lewin", "Physics", "MIT", new DateOnly(1936, 1, 29));
        catalogueDbContext.Scientists.Add(walterLewin);
        
        Scientist janVanParadijs = new Scientist("Jan Van Paradijs", "Physics", "University Of Amsterdam", new DateOnly(1946, 6, 9));
        catalogueDbContext.Scientists.Add(janVanParadijs);
        
        Scientist holgerPedersen = new Scientist("Holger Pedersen", "Physics", "Niels Bohr Institute", new DateOnly(1946, 11, 3));
        catalogueDbContext.Scientists.Add(holgerPedersen);
        
        Scientist paulJoss = new Scientist("Paul C. Joss", "Physics", "MIT");
        catalogueDbContext.Scientists.Add(paulJoss);

        Scientist charlesWarwick = new Scientist("Charles Warwick", "Neuroscience", "University of Pittsburgh");
        catalogueDbContext.Scientists.Add(charlesWarwick);

        Scientist anelaChoy = new Scientist("Anela Choy", "Oceanography", "UC San Diego");
        catalogueDbContext.Scientists.Add(anelaChoy);
        
        Scientist robSherlock = new Scientist("Robert E. Sherlock", "Research", "MBARI", new DateOnly(1966, 11, 1));
        catalogueDbContext.Scientists.Add(robSherlock);

        foreach (Scientist scientist in catalogueDbContext.Scientists)
        {
            scientist.ScientistId = catalogueDbContext.Scientists.IndexOf(scientist) + 1;
        }
    }

    private static void SeedJournals(CatalogueDbContext catalogueDbContext)
    {
        ScienceJournal journalNature = new ScienceJournal("Nature");
        catalogueDbContext.Journals.Add(journalNature);

        ScienceJournal journalScAdvances = new ScienceJournal("Science Advances", 15.00);
        catalogueDbContext.Journals.Add(journalScAdvances);
        
        foreach (ScienceJournal journal in catalogueDbContext.Journals)
        {
            journal.JournalId = catalogueDbContext.Journals.IndexOf(journal) + 1;
        }
    }

    private static void SeedArticles(CatalogueDbContext catalogueDbContext)
    {
        ScientificArticle articleOrbitalPeriodXRayBurster =
            new ScientificArticle("A four-hour orbital period of the X-ray burster 4U/MXB1636—53",
                [catalogueDbContext.Scientists[0], catalogueDbContext.Scientists[1], catalogueDbContext.Scientists[2]], // TODO: ask teacher about collection expression
                new DateOnly(1981, 12, 31), 3,
                ArticleCategory.Astrophysics, catalogueDbContext.Journals[0]);
        catalogueDbContext.Articles.Add(articleOrbitalPeriodXRayBurster);

        ScientificArticle articleXRayBurstSources =
            new ScientificArticle("X-ray burst sources", [catalogueDbContext.Scientists[0], catalogueDbContext.Scientists[3]],
                new DateOnly(1977, 11, 17), 6, ArticleCategory.Astrophysics, catalogueDbContext.Journals[0]);
        catalogueDbContext.Articles.Add(articleXRayBurstSources);
        
        ScientificArticle articleKappa =
            new ScientificArticle("Kappa opioids inhibit spinal output neurons to suppress itch", [catalogueDbContext.Scientists[4]],
                new DateOnly(2024, 09, 25), 18, ArticleCategory.Neuroscience, catalogueDbContext.Journals[1]);
        catalogueDbContext.Articles.Add(articleKappa);

        ScientificArticle articleLarvaceans =
            new ScientificArticle(
                "From the surface to the seafloor: How giant larvaceans transport microplastics into the deep sea",
                [catalogueDbContext.Scientists[5], catalogueDbContext.Scientists[6]], new DateOnly(2017, 8, 16), 5,
                ArticleCategory.MarineEcology, catalogueDbContext.Journals[1]);
        catalogueDbContext.Articles.Add(articleLarvaceans);
        
        foreach (ScientificArticle article in catalogueDbContext.Articles)
        {
            article.ArticleId = catalogueDbContext.Articles.IndexOf(article) + 1;
        }
    }

    public static void Seed(CatalogueDbContext catalogueDbContext)
    {
        SeedScientists(catalogueDbContext);
        SeedJournals(catalogueDbContext);
        SeedArticles(catalogueDbContext);
        catalogueDbContext.SaveChanges();
        
    }
}*/