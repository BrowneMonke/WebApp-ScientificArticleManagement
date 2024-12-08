using ArticleManagement.BL.Domain;

namespace ArticleManagement.DAL;

public interface IRepository
{
    IEnumerable<ScientificArticle> ReadAllArticles();
    IEnumerable<ScientificArticle> ReadAllArticlesWithAuthorsAndJournals();
    IEnumerable<ScientificArticle> ReadArticlesByCategory(ArticleCategory categoryChoice);
    public IEnumerable<ScientificArticle> ReadArticlesByCategoryWithAuthorsAndJournals(ArticleCategory categoryChoice);
    public IEnumerable<ScientificArticle> ReadArticlesOfScientist(int scientistId);
    ScientificArticle ReadArticle(int id);
    void CreateArticle(ScientificArticle articleToInsert);

    IEnumerable<Scientist> ReadAllScientists();
    IEnumerable<Scientist> ReadAllScientistsWithArticles();
    IEnumerable<Scientist> ReadScientistsByNameAndDateOfBirth(string nameString, DateOnly? dateOfBirth);
    Scientist ReadScientist(int id);
    void CreateScientist(Scientist scientistToInsert);

    void CreateArticleScientistLink(ArticleScientistLink articleScientistLink);
    void DeleteArticleScientistLink(int articleId, int scientistId);
    IEnumerable<ArticleScientistLink> ReadArticleScientistLinksByArticleId(int articleId);
    ArticleScientistLink ReadArticleScientistLinkByArticleIdAndScientistId(int articleId, int scientistId);
}