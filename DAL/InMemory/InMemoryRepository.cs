using ArticleManagement.BL.Domain;
using Microsoft.AspNetCore.Identity;

namespace ArticleManagement.DAL.InMemory;

public class InMemoryRepository : IRepository
{
    private static readonly List<Scientist> Scientists = [];
    private static readonly List<ScientificArticle> Articles = [];
    private static readonly List<ScienceJournal> Journals = [];

    public IEnumerable<ScientificArticle> ReadAllArticles()
    {
        return Articles;
    }

    public IEnumerable<ScientificArticle> ReadAllArticlesWithAuthorsAndJournals()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<ScientificArticle> ReadArticlesByCategory(ArticleCategory categoryChoice)
    {
        List<ScientificArticle> articlesOfCategory = [];
        foreach (ScientificArticle article in Articles)
        {
            if (article.Category == categoryChoice)
            {
                articlesOfCategory.Add(article);
            }
        }

        return articlesOfCategory;
    }

    public IEnumerable<ScientificArticle> ReadArticlesByCategoryWithAuthorsAndJournals(ArticleCategory categoryChoice)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<ScientificArticle> ReadArticlesByScientist(int scientistId)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<ScientificArticle> ReadArticlesNotByScientist(int scientistId)
    {
        throw new NotImplementedException();
    }

    public ScientificArticle ReadArticleById(int articleId)
    {
        return Articles[articleId - 1];
    }

    public ScientificArticle ReadArticleByIdWithDataOwner(int id)
    {
        throw new NotImplementedException();
    }

    public ScientificArticle ReadArticleByIdWithAuthorsAndJournal(int id)
    {
        throw new NotImplementedException();
    }

    public ScientificArticle ReadArticleByIdWithAuthorsAndJournalAndDataOwner(int id)
    {
        throw new NotImplementedException();
    }

    public void CreateArticle(ScientificArticle articleToInsert)
    {
        if (Articles == null || Articles.Count == 0)
        {
            articleToInsert.Id = 1;
        }
        else
        {
            articleToInsert.Id = Articles.Last().Id + 1;
        }
        Articles.Add(articleToInsert);
    }

    public void UpdateArticle(ScientificArticle scientificArticle)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Scientist> ReadAllScientists()
    {
        return Scientists;
    }

    public IEnumerable<Scientist> ReadAllScientistsWithArticles()
    {
        throw new NotImplementedException();
    }

    private static bool MatchScientistName(string nameString, Scientist scientist)
    {
        string[] scientistNameParts = nameString.Split(" ");

        foreach (var namePart in scientistNameParts)
        {
            bool isMatching = scientist.Name.ToLower().Contains(namePart.ToLower());
            if (!isMatching) return false;
        }

        return true;
    }
    
    private static void CheckNameFilter(string nameString, List<Scientist> filteredScientistsList)
    {
        if (nameString == null || nameString.Trim() == "") return;
        
        foreach (Scientist scientist in Scientists)
        {
            bool isMatching = MatchScientistName(nameString, scientist);
            
            if (isMatching) filteredScientistsList.Add(scientist);
        }
    }
    
    private static void CheckDobFilter(DateOnly? dateOfBirth, List<Scientist> filteredScientistsList)
    {
        if (dateOfBirth == null) return;
        foreach (Scientist scientist in Scientists)
        {
            if (scientist.DateOfBirth == dateOfBirth)
            {
                filteredScientistsList.Add(scientist);
            }
        }
    }
    
    public IEnumerable<Scientist> ReadScientistsByNameAndDateOfBirthWithArticles(string nameString, DateOnly? dateOfBirth)
    {
        List<Scientist> filteredScientistsList = [];
        CheckNameFilter(nameString, filteredScientistsList);
        CheckDobFilter(dateOfBirth, filteredScientistsList);
        return filteredScientistsList;
    }

    public Scientist ReadScientistById(int scientistId)
    {
        return Scientists[scientistId - 1];
    }

    public void CreateScientist(Scientist scientistToInsert)
    {
        if (Scientists == null || Scientists.Count == 0)
        {
            scientistToInsert.Id = 1;
        }
        else
        {
            scientistToInsert.Id = Scientists.Last().Id + 1;
        }
        Scientists.Add(scientistToInsert);
    }

    public void CreateArticleScientistLink(ArticleScientistLink articleScientistLink)
    {
        throw new NotImplementedException();
    }
    
    public void DeleteArticleScientistLink(ArticleScientistLink articleScientistLink)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<ArticleScientistLink> ReadArticleScientistLinksByArticleId(int articleId)
    {
        throw new NotImplementedException();
    }
    

    public ArticleScientistLink ReadArticleScientistLinkByArticleIdAndScientistId(int articleId, int scientistId)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<ScienceJournal> ReadAllJournals()
    {
        return Journals;
    }

    public ScienceJournal ReadJournalByIdWithArticles(int id)
    {
        throw new NotImplementedException();
    }

    public void CreateJournal(ScienceJournal journalToInsert)
    {
        throw new NotImplementedException();
    }

    public IdentityUser GetUserByUserName(string userName)
    {
        throw new NotImplementedException();
    }


    private static void SeedScientists()
    {
        Scientist walterLewin = new Scientist("Walter H. G. Lewin", "Physics", "MIT", new DateOnly(1936, 1, 29));
        Scientist janVanParadijs = new Scientist("Jan Van Paradijs", "Physics", "University Of Amsterdam", new DateOnly(1946, 6, 9));
        Scientist holgerPedersen = new Scientist("Holger Pedersen", "Physics", "Niels Bohr Institute", new DateOnly(1946, 11, 3));
        Scientist paulJoss = new Scientist("Paul C. Joss", "Physics", "MIT");
        Scientist charlesWarwick = new Scientist("Charles Warwick", "Neuroscience", "University of Pittsburgh");
        Scientist anelaChoy = new Scientist("Anela Choy", "Oceanography", "UC San Diego");
        Scientist robSherlock = new Scientist("Robert E. Sherlock", "Research", "MBARI", new DateOnly(1966, 11, 1));

        Scientists.AddRange([walterLewin, janVanParadijs, holgerPedersen, paulJoss, charlesWarwick, anelaChoy, robSherlock]);
        
        foreach (Scientist scientist in Scientists)
        {
            scientist.Id = Scientists.IndexOf(scientist) + 1;
        }
    }

    private static void SeedJournals()
    {
        ScienceJournal journalNature = new ScienceJournal("Nature");
        Journals.Add(journalNature);

        ScienceJournal journalScAdvances = new ScienceJournal("Science Advances", 15.00);
        Journals.Add(journalScAdvances);
        
        foreach (ScienceJournal journal in Journals)
        {
            journal.Id = Journals.IndexOf(journal) + 1;
        }
    }

    private static void SeedArticles()
    {
        List<ArticleScientistLink> authorsList = [];
        
        foreach (var scientist in Scientists)
        {
            authorsList.Add(new ArticleScientistLink{Scientist = scientist});
        }
        ScientificArticle articleOrbitalPeriodXRayBurster =
            new ScientificArticle("A four-hour orbital period of the X-ray burster 4U/MXB1636—53")
            {
                AuthorLinks = [authorsList[0], authorsList[1], authorsList[2]],
                DateOfPublication = new DateOnly(1981, 12, 31),
                NumberOfPages = 3,
                Category = ArticleCategory.Astrophysics,
                Journal = Journals[0]
            };
        Articles.Add(articleOrbitalPeriodXRayBurster);

        ScientificArticle articleXRayBurstSources =
            new ScientificArticle("X-ray burst sources")
            {
                AuthorLinks = [authorsList[0], authorsList[3]],
                DateOfPublication = new DateOnly(1977, 11, 17),
                NumberOfPages = 6,
                Category = ArticleCategory.Astrophysics,
                Journal = Journals[0]
            };
        Articles.Add(articleXRayBurstSources);

        ScientificArticle articleKappa =
            new ScientificArticle("Kappa opioids inhibit spinal output neurons to suppress itch")
            {
                AuthorLinks = [authorsList[4]],
                DateOfPublication = new DateOnly(2024, 09, 25),
                NumberOfPages = 18,
                Category = ArticleCategory.Neuroscience,
                Journal = Journals[1]
            };
        Articles.Add(articleKappa);

        ScientificArticle articleLarvaceans =
            new ScientificArticle("From the surface to the seafloor: How giant larvaceans transport microplastics into the deep sea")
            {
                AuthorLinks = [authorsList[5], authorsList[6]],
                DateOfPublication = new DateOnly(2017, 8, 16),
                NumberOfPages = 5,
                Category = ArticleCategory.MarineEcology,
                Journal = Journals[1]
            };
        Articles.Add(articleLarvaceans);
        
        foreach (ScientificArticle article in Articles)
        {
            article.Id = Articles.IndexOf(article) + 1;
        }
    }

    public static void Seed()
    {
        SeedScientists();
        SeedJournals();
        SeedArticles();
    }
    
}