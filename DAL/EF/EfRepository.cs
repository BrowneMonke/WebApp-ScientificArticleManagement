using ArticleManagement.BL.Domain;
using Microsoft.EntityFrameworkCore;

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

    public IEnumerable<ScientificArticle> ReadAllArticlesWithAuthorsAndJournals()
    {
        IQueryable<ScientificArticle> articles = _catalogueDbContext.Articles
                                                                    .Include(art => art.Journal)
                                                                    .Include(art => art.AuthorLinks)
                                                                    .ThenInclude(authLk => authLk.Scientist);
        return articles.ToList();
    }

    public IEnumerable<ScientificArticle> ReadArticlesByCategory(ArticleCategory categoryChoice)
    {
        return _catalogueDbContext.Articles.Where(article => article.Category == categoryChoice).ToList();
    }
    public IEnumerable<ScientificArticle> ReadArticlesByCategoryWithAuthorsAndJournals(ArticleCategory categoryChoice)
    {
        IQueryable<ScientificArticle> articles = _catalogueDbContext.Articles
            .Include(art => art.Journal)
            .Include(art => art.AuthorLinks)
            .ThenInclude(authLk => authLk.Scientist);
        return articles.Where(article => article.Category == categoryChoice).ToList();
    }

    public IEnumerable<ScientificArticle> ReadArticlesOfScientist(int scientistId)
    {
        var articles = _catalogueDbContext.Articles
            .Where(article => article.AuthorLinks.Any(authLk => authLk.Scientist.ScientistId == scientistId));

        return articles.ToList();
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

    public IEnumerable<Scientist> ReadAllScientistsWithArticles()
    {
        IQueryable<Scientist> scientists = _catalogueDbContext.Scientists
                                                              .Include(s => s.ArticleLinks)
                                                              .ThenInclude(al => al.Article);
        return scientists.ToList();
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

    public void CreateArticleScientistLink(ArticleScientistLink articleScientistLink)
    {
        _catalogueDbContext.ArticleScientistLinks.Add(articleScientistLink);
        _catalogueDbContext.SaveChanges();
    }

    public void DeleteArticleScientistLink(int articleId, int scientistId)
    {
        ArticleScientistLink linkToDelete = _catalogueDbContext.ArticleScientistLinks
            .Where(artScLk => artScLk.Article.ArticleId == articleId)
            .SingleOrDefault(artScLk => artScLk.Scientist.ScientistId == scientistId);

        if (linkToDelete != null) _catalogueDbContext.ArticleScientistLinks.Remove(linkToDelete);
        _catalogueDbContext.SaveChanges();
    }

    public ArticleScientistLink ReadArticleScientistLinkByArticleIdAndScientistId(int articleId, int scientistId)
    {
        return _catalogueDbContext.ArticleScientistLinks.Find(articleId, scientistId);
    }
}