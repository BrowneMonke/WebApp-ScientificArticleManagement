using ArticleManagement.BL.Domain;

namespace ArticleManagement.DAL.EF;

public static class DataSeeder
{
    public static void Seed(CatalogueDbContext catalogueDbContext)
    {
        Scientist walterLewin = new Scientist("Walter H. G. Lewin", "Physics", "MIT", new DateOnly(1936, 1, 29));
        Scientist janVanParadijs = new Scientist("Jan Van Paradijs", "Physics", "University Of Amsterdam", new DateOnly(1946, 6, 9));
        Scientist holgerPedersen = new Scientist("Holger Pedersen", "Physics", "Niels Bohr Institute", new DateOnly(1946, 11, 3));
        Scientist paulJoss = new Scientist("Paul C. Joss", "Physics", "MIT");
        Scientist charlesWarwick = new Scientist("Charles Warwick", "Neuroscience", "University of Pittsburgh");
        Scientist anelaChoy = new Scientist("Anela Choy", "Oceanography", "UC San Diego");
        Scientist robSherlock = new Scientist("Robert E. Sherlock", "Research", "MBARI", new DateOnly(1966, 11, 1));


        ScienceJournal journalNature = new ScienceJournal("Nature");
        ScienceJournal journalScAdvances = new ScienceJournal("Science Advances", 15.00);


        ScientificArticle articleOrbitalPeriodXRayBurster = new ScientificArticle("A four-hour orbital period of the X-ray burster 4U/MXB1636—53")
        {
            DateOfPublication = new DateOnly(1981, 12, 31),
            NumberOfPages = 3,
            Category = ArticleCategory.Astrophysics,
            Journal = journalNature
        };
        ArticleScientistLink orbitalPeriodWalterLewin = new ArticleScientistLink() { Article = articleOrbitalPeriodXRayBurster, Scientist = walterLewin, IsLeadResearcher = true};
        ArticleScientistLink orbitalPeriodJanVanParadijs = new ArticleScientistLink() { Article = articleOrbitalPeriodXRayBurster, Scientist = janVanParadijs };
        ArticleScientistLink orbitalPeriodHolgerPederson = new ArticleScientistLink() { Article = articleOrbitalPeriodXRayBurster, Scientist = holgerPedersen };
        articleOrbitalPeriodXRayBurster.AuthorLinks = [orbitalPeriodWalterLewin, orbitalPeriodJanVanParadijs, orbitalPeriodHolgerPederson];        

        ScientificArticle articleXRayBurstSources = new ScientificArticle("X-ray burst sources") 
        {
            DateOfPublication = new DateOnly(1977, 11, 17),
            NumberOfPages = 6,
            Category = ArticleCategory.Astrophysics,
            Journal = journalNature
        };
        ArticleScientistLink xRayBurstSourcesWalterLewin = new ArticleScientistLink() { Article = articleXRayBurstSources, Scientist = walterLewin, IsLeadResearcher = true};
        ArticleScientistLink xRayBurstSourcesPaulJoss = new ArticleScientistLink() { Article = articleXRayBurstSources, Scientist = paulJoss };
        articleXRayBurstSources.AuthorLinks = [xRayBurstSourcesWalterLewin, xRayBurstSourcesPaulJoss];

        ScientificArticle articleKappa = new ScientificArticle("Kappa opioids inhibit spinal output neurons to suppress itch")
        {
            DateOfPublication = new DateOnly(2024, 09, 25),
            NumberOfPages = 18,
            Category = ArticleCategory.Neuroscience,
            Journal = journalScAdvances
        };
        ArticleScientistLink kappaCharlesWar = new ArticleScientistLink() { Article = articleKappa, Scientist = charlesWarwick, IsLeadResearcher = true};
        ArticleScientistLink kappaHolgerPeder = new ArticleScientistLink() { Article = articleKappa, Scientist = holgerPedersen};
        articleKappa.AuthorLinks = [kappaCharlesWar, kappaHolgerPeder];
        
        ScientificArticle articleLarvaceans = new ScientificArticle("From the surface to the seafloor: How giant larvaceans transport microplastics into the deep sea")
        {
            DateOfPublication = new DateOnly(2017, 8, 16),
            NumberOfPages = 5,
            Category = ArticleCategory.MarineEcology,
            Journal = journalScAdvances
        };
        ArticleScientistLink larvaceansAnelaChoy = new ArticleScientistLink() { Article = articleLarvaceans, Scientist = anelaChoy };
        ArticleScientistLink larvaceansRobSherlock = new ArticleScientistLink() { Article = articleLarvaceans, Scientist = robSherlock, IsLeadResearcher = true};
        articleLarvaceans.AuthorLinks = [larvaceansAnelaChoy, larvaceansRobSherlock];
        
        ScientificArticle[] seedArticles = [articleOrbitalPeriodXRayBurster, articleXRayBurstSources, articleKappa, articleLarvaceans];
        foreach (var article in seedArticles)
        {
            foreach (var linkArticleScientist in article.AuthorLinks)
            {
                linkArticleScientist.Scientist.ArticleLinks.Add(linkArticleScientist);
                catalogueDbContext.ArticleScientistLinks.Add(linkArticleScientist);
            }
        }
        catalogueDbContext.Scientists.AddRange(walterLewin, janVanParadijs, holgerPedersen, paulJoss, charlesWarwick, anelaChoy, robSherlock);
        catalogueDbContext.Journals.AddRange(journalNature, journalScAdvances);
        catalogueDbContext.Articles.AddRange(seedArticles);

        catalogueDbContext.SaveChanges();
        catalogueDbContext.ChangeTracker.Clear();
    }
}