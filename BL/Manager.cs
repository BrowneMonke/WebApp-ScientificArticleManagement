using ArticleManagement.BL.Domain;
using ArticleManagement.DAL;

namespace ArticleManagement.BL;

public class Manager : IManager
{
    private readonly IRepository _repository;

    public Manager(IRepository repository)
    {
        _repository = repository;
    }
    
    public IEnumerable<ScientificArticle> GetAllArticles()
    {
        return _repository.ReadAllArticles();
    }

    public IEnumerable<ScientificArticle> GetArticlesByCategory(int categoryChoice)
    {
        return _repository.ReadArticlesByCategory(categoryChoice);
    }

    public ScientificArticle GetArticle(int articleId)
    {
        return _repository.ReadArticle(articleId);
    }

    public ScientificArticle AddArticle(string title, ICollection<Scientist> authors, DateOnly dateOfPublication, int numberOfPages, ArticleCategory category, ScienceJournal journal)
    {
        ScientificArticle article = new ScientificArticle(title, authors, dateOfPublication, numberOfPages, category, journal);
        _repository.CreateArticle(article);
        return article;
    }

    public IEnumerable<Scientist> GetAllScientists()
    {
        return _repository.ReadAllScientists();
    }

    public IEnumerable<Scientist> GetScientistsByNameAndDateOfBirth(string nameString, string dobString)
    {
        return _repository.ReadScientistsByNameAndDateOfBirth(nameString, dobString);
    }

    public Scientist GetScientist(int scientistId)
    {
        return _repository.ReadScientist(scientistId);
    }

    public Scientist AddScientist(string name, string faculty, string university, DateOnly? dateOfBirth = null)
    {
        Scientist scientist = new Scientist(name, faculty, university, dateOfBirth);
        _repository.CreateScientist(scientist);
        return scientist;
    }
}