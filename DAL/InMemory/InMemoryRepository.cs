using System.Text.RegularExpressions;
using ArticleManagement.BL.Domain;

namespace ArticleManagement.DAL;

public class InMemoryRepository : IRepository
{
    private static readonly List<Scientist> Scientists = [];
    private static readonly List<ScientificArticle> Articles = [];
    private static readonly List<ScienceJournal> Journals = [];

    public IEnumerable<ScientificArticle> ReadAllArticles()
    {
        return Articles;
    }

    public IEnumerable<ScientificArticle> ReadArticlesByCategory(ArticleCategory categoryChoice)
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

    public ScientificArticle ReadArticle(int articleId)
    {
        return Articles[articleId - 1];
    }

    public void CreateArticle(ScientificArticle articleToInsert)
    {
        if (Articles == null || Articles.Count == 0)
        {
            articleToInsert.ArticleId = 1;
        }
        else
        {
            articleToInsert.ArticleId = Articles.Last().ArticleId + 1;
        }
        Articles.Add(articleToInsert);
    }

    public IEnumerable<Scientist> ReadAllScientists()
    {
        return Scientists;
    }

    public bool MatchScientistName(string nameString, Scientist scientist)
    {
        string[] scientistNameParts = nameString.Split(" ");

        foreach (var namePart in scientistNameParts)
        {
            bool isMatching = scientist.Name.ToLower().Contains(namePart.ToLower());
            if (!isMatching) return false;
        }

        return true;
    }
    
    private void CheckNameFilter(string nameString, List<Scientist> filteredScientistsList)
    {
        if (nameString == null || nameString.Trim() == "") return;
        
        foreach (Scientist scientist in Scientists)
        {
            bool isMatching = MatchScientistName(nameString, scientist);
            
            if (isMatching) filteredScientistsList.Add(scientist);
        }
    }
    
    private void CheckDobFilter(string dobString, List<Scientist> filteredScientistsList)
    {
        if (!DateOnly.TryParse(dobString, out DateOnly dateOfBirth)) return;
        foreach (Scientist scientist in Scientists)
        {
            if (scientist.DateOfBirth == dateOfBirth)
            {
                filteredScientistsList.Add(scientist);
            }
        }
    }
    
    public IEnumerable<Scientist> ReadScientistsByNameAndDateOfBirth(string nameString, string dobString)
    {
        if (nameString.Trim() == "" && dobString.Trim() == "") return ReadAllScientists();
        List<Scientist> filteredScientistsList = [];
        CheckNameFilter(nameString, filteredScientistsList);
        CheckDobFilter(dobString, filteredScientistsList);
        return filteredScientistsList;
    }

    public Scientist ReadScientist(int scientistId)
    {
        return Scientists[scientistId - 1];
    }

    public void CreateScientist(Scientist scientistToInsert)
    {
        if (Scientists == null || Scientists.Count == 0)
        {
            scientistToInsert.ScientistId = 1;
        }
        else
        {
            scientistToInsert.ScientistId = Scientists.Last().ScientistId + 1;
        }
        Scientists.Add(scientistToInsert);
    }

    public IEnumerable<ScienceJournal> ReadAllJournals()
    {
        return Journals;
    }

    public ScienceJournal ReadJournal(int journalId)
    {
        return Journals[journalId - 1];
    }

    public void CreateJournal(ScienceJournal journalToInsert)
    {
        if (Journals == null || Journals.Count == 0)
        {
            journalToInsert.JournalId = 1;
        }
        else
        {
            journalToInsert.JournalId = Journals.Last().JournalId + 1;
        }
        Journals.Add(journalToInsert);
    }

    
    private static void SeedScientists()
    {
        Scientist walterLewin = new Scientist("Walter H. G. Lewin", "Physics", "MIT", new DateOnly(1936, 1, 29));
        Scientists.Add(walterLewin);
        
        Scientist janVanParadijs = new Scientist("Jan Van Paradijs", "Physics", "University Of Amsterdam", new DateOnly(1946, 6, 9));
        Scientists.Add(janVanParadijs);
        
        Scientist holgerPedersen = new Scientist("Holger Pedersen", "Physics", "Niels Bohr Institute", new DateOnly(1946, 11, 3));
        Scientists.Add(holgerPedersen);
        
        Scientist paulJoss = new Scientist("Paul C. Joss", "Physics", "MIT");
        Scientists.Add(paulJoss);

        Scientist charlesWarwick = new Scientist("Charles Warwick", "Neuroscience", "University of Pittsburgh");
        Scientists.Add(charlesWarwick);

        Scientist anelaChoy = new Scientist("Anela Choy", "Oceanography", "UC San Diego");
        Scientists.Add(anelaChoy);
        
        Scientist robSherlock = new Scientist("Robert E. Sherlock", "Research", "MBARI", new DateOnly(1966, 11, 1));
        Scientists.Add(robSherlock);

        foreach (Scientist scientist in Scientists)
        {
            scientist.ScientistId = Scientists.IndexOf(scientist) + 1;
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
            journal.JournalId = Journals.IndexOf(journal) + 1;
        }
    }

    private static void SeedArticles()
    {
        ScientificArticle articleOrbitalPeriodXRayBurster =
            new ScientificArticle("A four-hour orbital period of the X-ray burster 4U/MXB1636—53",
                [Scientists[0], Scientists[1], Scientists[2]], // TODO: ask teacher about collection expression
                new DateOnly(1981, 12, 31), 3,
                ArticleCategory.Astrophysics, Journals[0]);
        Articles.Add(articleOrbitalPeriodXRayBurster);

        ScientificArticle articleXRayBurstSources =
            new ScientificArticle("X-ray burst sources", [Scientists[0], Scientists[3]],
                new DateOnly(1977, 11, 17), 6, ArticleCategory.Astrophysics, Journals[0]);
        Articles.Add(articleXRayBurstSources);
        
        ScientificArticle articleKappa =
            new ScientificArticle("Kappa opioids inhibit spinal output neurons to suppress itch", [Scientists[4]],
                new DateOnly(2024, 09, 25), 18, ArticleCategory.Neuroscience, Journals[1]);
        Articles.Add(articleKappa);

        ScientificArticle articleLarvaceans =
            new ScientificArticle(
                "From the surface to the seafloor: How giant larvaceans transport microplastics into the deep sea",
                [Scientists[5], Scientists[6]], new DateOnly(2017, 8, 16), 5,
                ArticleCategory.MarineEcology, Journals[1]);
        Articles.Add(articleLarvaceans);
        
        foreach (ScientificArticle article in Articles)
        {
            article.ArticleId = Articles.IndexOf(article) + 1;
        }
    }

    public static void Seed()
    {
        SeedScientists();
        SeedJournals();
        SeedArticles();
    }
    
}