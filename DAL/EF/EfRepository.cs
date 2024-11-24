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
        return _catalogueDbContext.Articles.Where(article => article.Category == categoryChoice).ToList();
    }

    public ScientificArticle ReadArticle(int id)
    {
        return _catalogueDbContext.Articles.Find(id);
    }

    public void CreateArticle(ScientificArticle articleToInsert)
    {
        _catalogueDbContext.Articles.Add(articleToInsert);
        _catalogueDbContext.SaveChanges();
    }

    public IEnumerable<Scientist> ReadAllScientists()
    {
        return _catalogueDbContext.Scientists.ToList();
    }

    public IEnumerable<Scientist> ReadScientistsByNameAndDateOfBirth(string nameString, DateOnly? dateOfBirth)
    {
        IQueryable<Scientist> query = _catalogueDbContext.Scientists;

        if (dateOfBirth != null && dateOfBirth < DateOnly.FromDateTime(DateTime.Now))
        {
            query = query.Where(scientist => scientist.DateOfBirth == dateOfBirth);
        }
        
        string[] scientistNameParts = nameString.Split(" ");
        if (!String.IsNullOrEmpty(scientistNameParts[0]))
        {
            foreach (var namePart in scientistNameParts)
            {
                query = query.Where(s =>s.Name.ToLower().Contains(namePart.ToLower()));
            }
        }

        return query.ToList();
    }

    public Scientist ReadScientist(int id)
    {
        return _catalogueDbContext.Scientists.Find(id);
    }

    public void CreateScientist(Scientist scientistToInsert)
    {
        _catalogueDbContext.Scientists.Add(scientistToInsert);
        _catalogueDbContext.SaveChanges();
    }
}