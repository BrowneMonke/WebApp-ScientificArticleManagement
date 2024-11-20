using System.ComponentModel.DataAnnotations;
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

    public IEnumerable<ScientificArticle> GetArticlesByCategory(ArticleCategory categoryChoice)
    {
        return _repository.ReadArticlesByCategory(categoryChoice);
    }

    public ScientificArticle GetArticle(int articleId)
    {
        return _repository.ReadArticle(articleId);
    }
    
    private static void RelateAuthors(ScientificArticle article)
    {
        foreach (ArticleScientist articleScientist in article.Authors)
        {
            ArticleScientist connectingInstance = new ArticleScientist();
            connectingInstance.Article = article;
            connectingInstance.Scientist = articleScientist.Scientist;
            articleScientist.Scientist.Articles.Add(connectingInstance);
            articleScientist.Article = article;
        }
    }       

    private static void RelateJournal(ScientificArticle article)
    {
        article.Journal?.Articles.Add(article);
    }

    public ScientificArticle AddArticle(string title, IEnumerable<Scientist> authors, DateOnly dateOfPublication, int numberOfPages, ArticleCategory categoryChoice, ScienceJournal journal)
    {
        List<ArticleScientist> articleScientists = [];
        foreach (Scientist scientist in authors)
        {
            ArticleScientist authorInstance = new ArticleScientist
            {
                Scientist = scientist
            };
            articleScientists.Add(authorInstance);
        }

        ScientificArticle article = new ScientificArticle(title)
        {
            Authors = articleScientists,
            DateOfPublication = dateOfPublication,
            NumberOfPages = numberOfPages,
            Category = categoryChoice,
            Journal = journal
        };
        
        ICollection<ValidationResult> errors = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(article, new ValidationContext(article), errors, validateAllProperties:true);
        if (!isValid)
        {
            throw new ValidationException(String.Join("|", errors.Select(err => err.ErrorMessage)));
        }
        _repository.CreateArticle(article);
        RelateAuthors(article);
        RelateJournal(article);
        
        return article;
    }

    public IEnumerable<Scientist> GetAllScientists()
    {
        return _repository.ReadAllScientists();
    }

    public IEnumerable<Scientist> GetScientistsByNameAndDateOfBirth(string nameString, DateOnly? dateOfBirth)
    {
        if (nameString.Trim() == "" && dateOfBirth == null) return _repository.ReadAllScientists();
        return _repository.ReadScientistsByNameAndDateOfBirth(nameString, dateOfBirth);
    }

    public Scientist GetScientist(int scientistId)
    {
        return _repository.ReadScientist(scientistId);
    }
    
    public Scientist AddScientist(string name, string faculty, string university, DateOnly? dateOfBirth = null)
    {
        Scientist scientist = new Scientist(name, faculty, university, dateOfBirth);
        ICollection<ValidationResult> errors = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(scientist, new ValidationContext(scientist), errors, validateAllProperties:true);
        if (!isValid)
        {
            throw new ValidationException(String.Join("|", errors.Select(err => err.ErrorMessage)));
        }
        _repository.CreateScientist(scientist);
        return scientist;
    }

    
}