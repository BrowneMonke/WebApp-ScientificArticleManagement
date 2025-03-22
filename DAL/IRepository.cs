using ArticleManagement.BL.Domain;
using Microsoft.AspNetCore.Identity;

namespace ArticleManagement.DAL;

public interface IRepository
{
    IEnumerable<ScientificArticle> ReadAllArticles();
    IEnumerable<ScientificArticle> ReadAllArticlesWithAuthorsAndJournals();
    public IEnumerable<ScientificArticle> ReadArticlesByCategoryWithAuthorsAndJournals(ArticleCategory categoryChoice);
    public IEnumerable<ScientificArticle> ReadArticlesByScientist(int scientistId);
    public IEnumerable<ScientificArticle> ReadArticlesNotByScientist(int scientistId);
    ScientificArticle ReadArticleById(int id);
    ScientificArticle ReadArticleByIdWithDataOwner(int id);
    public ScientificArticle ReadArticleByIdWithAuthorsAndJournal(int id);
    public ScientificArticle ReadArticleByIdWithAuthorsAndJournalAndDataOwner(int id);
    void CreateArticle(ScientificArticle articleToInsert);
    public void UpdateArticle(ScientificArticle scientificArticle);

    IEnumerable<Scientist> ReadAllScientists();
    IEnumerable<Scientist> ReadAllScientistsWithArticles();
    IEnumerable<Scientist> ReadScientistsByNameAndDateOfBirthWithArticles(string nameString, DateOnly? dateOfBirth);
    Scientist ReadScientistById(int id);
    void CreateScientist(Scientist scientistToInsert);

    IEnumerable<ArticleScientistLink> ReadArticleScientistLinksByArticleId(int articleId);
    ArticleScientistLink ReadArticleScientistLinkByArticleIdAndScientistId(int articleId, int scientistId);
    void CreateArticleScientistLink(ArticleScientistLink articleScientistLink);
    void DeleteArticleScientistLink(ArticleScientistLink articleScientistLink);

    IEnumerable<ScienceJournal> ReadAllJournals();
    ScienceJournal ReadJournalByIdWithArticles(int id);
    void CreateJournal(ScienceJournal journalToInsert);

    IdentityUser ReadUserByUserName(string userName);
    
}