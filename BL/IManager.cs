using ArticleManagement.BL.Domain;

namespace ArticleManagement.BL;

public interface IManager
{
    IEnumerable<ScientificArticle> GetAllArticles();
    IEnumerable<ScientificArticle> GetArticlesByCategory(int categoryChoice);
    ScientificArticle GetArticle(int articleId);
    ScientificArticle AddArticle(string title, ICollection<Scientist> authors, DateOnly dateOfPublication, int numberOfPages, ArticleCategory category, ScienceJournal journal);
    
    IEnumerable<Scientist> GetAllScientists();
    IEnumerable<Scientist> GetScientistsByNameAndDateOfBirth(string nameString, string dobString);
    Scientist GetScientist(int scientistId);
    Scientist AddScientist(string name, string faculty, string university, DateOnly? dateOfBirth = null);
}