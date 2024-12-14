using ArticleManagement.BL.Domain;

namespace ArticleManagement.BL;

public interface IManager
{
    IEnumerable<ScientificArticle> GetAllArticles();
    IEnumerable<ScientificArticle> GetAllArticlesWithAuthorsAndJournals();
    IEnumerable<ScientificArticle> GetArticlesByCategory(ArticleCategory categoryChoice);
    public IEnumerable<ScientificArticle> GetArticlesByCategoryWithAuthorsAndJournals(ArticleCategory categoryChoice);
    IEnumerable<ScientificArticle> GetArticlesOfScientist(int scientistId);
    ScientificArticle GetArticle(int articleId);
    public ScientificArticle GetArticleByIdWithAuthorsAndJournal(int articleId);
    ScientificArticle AddArticle(string title, IEnumerable<Scientist> authors, DateOnly dateOfPublication, int numberOfPages, ArticleCategory category, ScienceJournal journal = null);
    
    IEnumerable<Scientist> GetAllScientists();
    IEnumerable<Scientist> GetAllScientistsWithArticles();
    IEnumerable<Scientist> GetScientistsByNameAndDateOfBirth(string nameString, DateOnly? dateOfBirth);
    Scientist GetScientist(int scientistId);
    Scientist AddScientist(string name, string faculty, string university, DateOnly? dateOfBirth = null);
    
    ArticleScientistLink AddArticleScientistLink(int articleId, int scientistId, bool isLeadResearcher = false);
    void RemoveArticleScientistLink(int articleId, int scientistId);

}