using ArticleManagement.BL.Domain;
using Microsoft.AspNetCore.Identity;

namespace ArticleManagement.DAL.EF;

public static class DataSeeder
{
    public static void Seed(ArticleDbContext articleDbContext)
    {
        // get identity users
        // var defaultUser = articleDbContext.Users.SingleOrDefault(u => u.UserName == "defaultuser@kdg.be"); // owns 0 articles
        var bob = articleDbContext.Users.SingleOrDefault(u => u.UserName == "bob@kdg.be");
        var marley = articleDbContext.Users.SingleOrDefault(u => u.UserName == "marley@kdg.be");
        var ross = articleDbContext.Users.SingleOrDefault(u => u.UserName == "ross@kdg.be");
        var deBouwer = articleDbContext.Users.SingleOrDefault(u => u.UserName == "de_bouwer@kdg.be");
        
        Scientist walterLewin = new Scientist("Walter H. G. Lewin", "Physics", "MIT", new DateOnly(1936, 1, 29));
        Scientist janVanParadijs = new Scientist("Jan Van Paradijs", "Physics", "University Of Amsterdam", new DateOnly(1946, 6, 9));
        Scientist holgerPedersen = new Scientist("Holger Pedersen", "Physics", "Niels Bohr Institute", new DateOnly(1946, 11, 3));
        Scientist paulJoss = new Scientist("Paul C. Joss", "Physics", "MIT");
        Scientist charlesWarwick = new Scientist("Charles Warwick", "Neuroscience", "University of Pittsburgh");
        Scientist anelaChoy = new Scientist("Anela Choy", "Oceanography", "UC San Diego");
        Scientist robSherlock = new Scientist("Robert E. Sherlock", "Research", "MBARI", new DateOnly(1966, 11, 1));
        Scientist masahiroEri = new Scientist("Masahiro Eriguchi", "Nephrology", "Nara Medical University", new DateOnly(1979, 12, 4));
        Scientist yuLi = new Scientist("Yu Li", "Cardiology", "Zhejiang University School of Medicine");
        Scientist xiaoLiu = new Scientist("Xiaoli Liu", "Neurology", "Zhejiang University School of Medicine", new DateOnly(1985, 2, 7));
        Scientist robinRogers = new Scientist("Robin D. Rogers", "Chemistry", "University of Alabama");
        Scientist keithComb = new Scientist("Keith D. Combrink", "Chemistry", "University of Kansas", new DateOnly(1965, 5, 17));
        Scientist arthurVes = new Scientist("Arthur Vesperini", "Physics", "University of Siena", new DateOnly(1977, 4, 12));
        Scientist robertoFranz = new Scientist("Roberto Franzosi", "Physics", "University of Siena", new DateOnly(1982, 3, 11));


        ScienceJournal journalNature = new ScienceJournal("Nature")
        {
            CountryOfOrigin = Country.Uk,
            YearFounded = 1869
        };
        ScienceJournal journalScAdvances = new ScienceJournal("Science Advances", 15.00)
        {
            CountryOfOrigin = Country.Usa,
            YearFounded = 2015
        };
        ScienceJournal journalScImmunology = new ScienceJournal("Science Immunology", 21.89)
        {
            CountryOfOrigin = Country.Usa,
            YearFounded = 2016
        };
        ScienceJournal journalHelvetica = new ScienceJournal("Helvetica Chimica Acta", 29.50)
        {
            CountryOfOrigin = Country.Switzerland,
            YearFounded = 1918
        };
        ScienceJournal journalAnnalen = new ScienceJournal("Annalen der Physik", 18)
        {
            CountryOfOrigin = Country.Germany,
            YearFounded = 1799
        };


        ScientificArticle articleOrbitalPeriodXRayBurster = new ScientificArticle("A four-hour orbital period of the X-ray burster 4U/MXB1636—53")
        {
            DateOfPublication = new DateOnly(1981, 12, 31),
            NumberOfPages = 3,
            Category = ArticleCategory.Astrophysics,
            Journal = journalNature,
            DataOwner = bob
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
            Journal = journalNature,
            DataOwner = bob
        };
        ArticleScientistLink xRayBurstSourcesWalterLewin = new ArticleScientistLink() { Article = articleXRayBurstSources, Scientist = walterLewin, IsLeadResearcher = true};
        ArticleScientistLink xRayBurstSourcesPaulJoss = new ArticleScientistLink() { Article = articleXRayBurstSources, Scientist = paulJoss };
        articleXRayBurstSources.AuthorLinks = [xRayBurstSourcesWalterLewin, xRayBurstSourcesPaulJoss];

        ScientificArticle articleKappa = new ScientificArticle("Kappa opioids inhibit spinal output neurons to suppress itch")
        {
            DateOfPublication = new DateOnly(2024, 09, 25),
            NumberOfPages = 18,
            Category = ArticleCategory.Neuroscience,
            Journal = journalScAdvances,
            DataOwner = marley
        };
        ArticleScientistLink kappaCharlesWar = new ArticleScientistLink() { Article = articleKappa, Scientist = charlesWarwick, IsLeadResearcher = true};
        ArticleScientistLink kappaHolgerPeder = new ArticleScientistLink() { Article = articleKappa, Scientist = holgerPedersen};
        articleKappa.AuthorLinks = [kappaCharlesWar, kappaHolgerPeder];
        
        ScientificArticle articleLarvaceans = new ScientificArticle("From the surface to the seafloor: How giant larvaceans transport microplastics into the deep sea")
        {
            DateOfPublication = new DateOnly(2017, 8, 16),
            NumberOfPages = 5,
            Category = ArticleCategory.MarineEcology,
            Journal = journalScAdvances,
            DataOwner = ross
        };
        ArticleScientistLink larvaceansAnelaChoy = new ArticleScientistLink() { Article = articleLarvaceans, Scientist = anelaChoy };
        ArticleScientistLink larvaceansRobSherlock = new ArticleScientistLink() { Article = articleLarvaceans, Scientist = robSherlock, IsLeadResearcher = true};
        articleLarvaceans.AuthorLinks = [larvaceansAnelaChoy, larvaceansRobSherlock];

        ScientificArticle articleAtpImmuneResponses =
            new ScientificArticle("ATP release drives heightened immune responses associated with hypertension")
            {
                DateOfPublication = new DateOnly(2019, 6, 7),
                NumberOfPages = 23,
                Category = ArticleCategory.Biology,
                Journal = journalScImmunology,
                DataOwner = deBouwer
            };
        ArticleScientistLink immuneResponsesMasahiro = new ArticleScientistLink() { Article = articleAtpImmuneResponses, Scientist = masahiroEri };
        ArticleScientistLink immuneResponsesYuLi = new ArticleScientistLink() { Article = articleAtpImmuneResponses, Scientist = yuLi, IsLeadResearcher = true};
        ArticleScientistLink immuneResponsesXiaoLiu = new ArticleScientistLink() { Article = articleAtpImmuneResponses, Scientist = xiaoLiu };
        articleAtpImmuneResponses.AuthorLinks = [immuneResponsesMasahiro, immuneResponsesYuLi, immuneResponsesXiaoLiu];

        ScientificArticle articleTaxanes =
            new ScientificArticle(
                "An Enantioselective Approach to the Taxanes: Direct access to functionalized cis-tricyclopentadecanes via  Wagner-Meerwein rearrangements")
            {
                DateOfPublication = new DateOnly(1992, 10, 2),
                NumberOfPages = 16,
                Category = ArticleCategory.Chemistry,
                Journal = journalHelvetica,
                DataOwner = deBouwer
            };
        ArticleScientistLink taxanesRobertRogers = new ArticleScientistLink() { Article = articleTaxanes, Scientist = robinRogers, IsLeadResearcher = true };
        ArticleScientistLink taxanesCombrink = new ArticleScientistLink() { Article = articleTaxanes, Scientist = keithComb };
        articleTaxanes.AuthorLinks = [taxanesRobertRogers, taxanesCombrink];

        ScientificArticle articleQuantumEntanglement =
            new ScientificArticle("Enhancing Quantum Entanglement Through Parametric Control of Atomic-Cavity States")
            {
                DateOfPublication = new DateOnly(2017, 8, 23),
                NumberOfPages = 14,
                Category = ArticleCategory.QuantumPhysics,
                Journal = journalAnnalen,
                DataOwner = ross
            };
        ArticleScientistLink quantumRoberto = new ArticleScientistLink() { Article = articleQuantumEntanglement, Scientist = robertoFranz };
        ArticleScientistLink quantumArthur = new ArticleScientistLink() { Article = articleQuantumEntanglement, Scientist = arthurVes, IsLeadResearcher = true};
        articleQuantumEntanglement.AuthorLinks = [quantumRoberto, quantumArthur];
        
        
        // no. of articles = 7 ; no. of journals = 5 ; no. of scientists = 14
        ScientificArticle[] seedArticles = [articleOrbitalPeriodXRayBurster, articleXRayBurstSources, articleKappa, articleLarvaceans, articleAtpImmuneResponses, articleTaxanes, articleQuantumEntanglement];
        foreach (var article in seedArticles)
        {
            foreach (var linkArticleScientist in article.AuthorLinks)
            {
                linkArticleScientist.Scientist.ArticleLinks.Add(linkArticleScientist);
                articleDbContext.ArticleScientistLinks.Add(linkArticleScientist);
            }
        }
        articleDbContext.Scientists.AddRange(walterLewin, janVanParadijs, holgerPedersen, paulJoss, charlesWarwick, anelaChoy, robSherlock);
        articleDbContext.Journals.AddRange(journalNature, journalScAdvances);
        articleDbContext.Articles.AddRange(seedArticles);
        
        // Save data
        articleDbContext.SaveChanges();
        // Clear tracked entities
        articleDbContext.ChangeTracker.Clear();
    }
}