using ArticleManagement.BL.Domain;

namespace ArticleManagement.DAL.EF;

public class EfRepository : IRepository
{
    private readonly CatalogueDbContext _catalogueDbContext;

    public EfRepository(CatalogueDbContext catalogueDbContext)
    {
        _catalogueDbContext = catalogueDbContext;
    }

    public IEnumerable<ScientificArticle> ReadAllArticles()
    {
        return _catalogueDbContext.Articles.ToList();
    }

    public IEnumerable<ScientificArticle> ReadArticlesByCategory(ArticleCategory categoryChoice)
    {
        List<ScientificArticle> articlesOfCategory = [];
        articlesOfCategory.AddRange(_catalogueDbContext.Articles.Where(article => article.Category == categoryChoice));

        return articlesOfCategory;
    }

    public ScientificArticle ReadArticle(int id)
    {
        return _catalogueDbContext.Articles.Find(id);
    }

    public void CreateArticle(ScientificArticle articleToInsert)
    {
        if (_catalogueDbContext.Articles == null || !_catalogueDbContext.Articles.Any())
        {
            articleToInsert.ArticleId = 1;
        }
        else
        {
            articleToInsert.ArticleId = _catalogueDbContext.Articles.ToList().Last().ArticleId + 1;
        }

        _catalogueDbContext.Articles.Add(articleToInsert);
        _catalogueDbContext.SaveChanges();
    }

    public IEnumerable<Scientist> ReadAllScientists()
    {
        return _catalogueDbContext.Scientists.ToList();
    }

    private IQueryable<Scientist> GetDobQuery(string dobString /*, List<Scientist> filteredScientistsList*/)
    {
        IQueryable<Scientist> query = _catalogueDbContext.Scientists;

        if (!DateOnly.TryParse(dobString, out DateOnly dateOfBirth)) return query;

        query = query.Where(scientist => scientist.DateOfBirth == dateOfBirth);
        return query;
        // filteredScientistsList.AddRange(query.ToList());
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

    private void FilterOnName(string nameString, IQueryable<Scientist> query, List<Scientist> filteredScientistsList)
    {
        /*if (nameString == null || nameString.Trim() == "")
        {
            filteredScientistsList.AddRange(query);
            return;
        }*/

        foreach (Scientist scientist in query)
        {
            bool isMatching = MatchScientistName(nameString, scientist);

            if (isMatching) filteredScientistsList.Add(scientist);
        }
    }

    public IEnumerable<Scientist> ReadScientistsByNameAndDateOfBirth(string nameString, string dobString)
    {
        List<Scientist> filteredScientistsList = [];

        IQueryable<Scientist> query = GetDobQuery(dobString);
        
        string firstNamePart = nameString.Split(' ')[0];
        if (!String.IsNullOrEmpty(firstNamePart))
        {
            query = query.Where(s => s.Name.ToLower().Contains(firstNamePart.ToLower()));
            FilterOnName(nameString, query, filteredScientistsList);
        }
        else
        {
            filteredScientistsList.AddRange(query);
        }

        return filteredScientistsList;
    }

    public Scientist ReadScientist(int id)
    {
        return _catalogueDbContext.Scientists.Find(id);
    }

    public void CreateScientist(Scientist scientistToInsert)
    {
        if (_catalogueDbContext.Scientists == null || !_catalogueDbContext.Scientists.Any())
        {
            scientistToInsert.ScientistId = 1;
        }
        else
        {
            scientistToInsert.ScientistId = _catalogueDbContext.Scientists.ToList().Last().ScientistId + 1;
        }

        _catalogueDbContext.Scientists.Add(scientistToInsert);
        _catalogueDbContext.SaveChanges();
    }
}