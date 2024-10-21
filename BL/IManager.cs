using ArticleManagement.BL.Domain;

namespace ArticleManagement.BL;

public interface IManager
{
    IEnumerable<ScientificArticle> GetAllArticles();
    void VerifyCategoryChoice(int categoryChoice);
    IEnumerable<ScientificArticle> GetArticlesByCategory(ArticleCategory categoryChoice);
    ScientificArticle GetArticle(int articleId);
    ScientificArticle AddArticle(string title, IEnumerable<Scientist> authors, DateOnly dateOfPublication, int numberOfPages, int category, ScienceJournal journal = null);
    
    IEnumerable<Scientist> GetAllScientists();
    IEnumerable<Scientist> GetScientistsByNameAndDateOfBirth(string nameString, string dobString);
    Scientist GetScientist(int scientistId);
    Scientist AddScientist(string name, string faculty, string university, DateOnly? dateOfBirth = null);
    bool TryParseScientist(string scientistNameString, out Scientist existingScientist);
}