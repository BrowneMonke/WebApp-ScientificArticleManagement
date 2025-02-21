using ArticleManagement.BL.Domain;

namespace ArticleManagement.BL;

public interface IManager
{
    IEnumerable<ScientificArticle> GetAllArticles();
    IEnumerable<ScientificArticle> GetAllArticlesWithAuthorsAndJournals();
    public IEnumerable<ScientificArticle> GetArticlesByCategoryWithAuthorsAndJournals(int categoryChoice);
    IEnumerable<ScientificArticle> GetArticlesByScientist(int scientistId);
    IEnumerable<ScientificArticle> GetArticlesNotByScientist(int scientistId);
    public ScientificArticle GetArticleByIdWithAuthorsAndJournal(int articleId);
    public ScientificArticle GetArticleByIdWithAuthorsAndJournalAndDataOwner(int articleId);
    ScientificArticle AddArticle(string title, IEnumerable<Scientist> authors, DateOnly dateOfPublication, int numberOfPages, ArticleCategory category, string userName, ScienceJournal journal = null);
    
    IEnumerable<Scientist> GetAllScientists();
    IEnumerable<Scientist> GetAllScientistsWithArticles();
    IEnumerable<Scientist> GetScientistsByNameAndDateOfBirthWithArticles(string nameString, DateOnly? dateOfBirth);
    Scientist GetScientistById(int scientistId);
    Scientist AddScientist(string name, string faculty, string university, DateOnly? dateOfBirth = null);

    public ArticleScientistLink GetArticleScientistLink(int articleId, int scientistId);
    ArticleScientistLink AddArticleScientistLink(int articleId, int scientistId, bool isLeadResearcher = false);
    void RemoveArticleScientistLink(int articleId, int scientistId);

    IEnumerable<ScienceJournal> GetAllJournals();
    ScienceJournal GetJournalByIdWithArticles(int id);
    ScienceJournal AddJournal(string name, int yearFounded, Country countryOfOrigin, double? price = null);

}