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

    public void VerifyCategoryChoice(int categoryChoice)
    {
        if (!Enum.IsDefined((ArticleCategory)categoryChoice))
        {
            throw new ArithmeticException("Invalid Category Number! Please try again.");
        }
    }

    public IEnumerable<ScientificArticle> GetArticlesByCategory(ArticleCategory categoryChoice)
    {
        return _repository.ReadArticlesByCategory(categoryChoice);
    }

    public ScientificArticle GetArticle(int articleId)
    {
        return _repository.ReadArticle(articleId);
    }

    public ScientificArticle AddArticle(string title, IEnumerable<Scientist> authors, DateOnly dateOfPublication, int numberOfPages, /*ArticleCategory*/int categoryChoice, ScienceJournal journal)
    {
        ArticleCategory category = (ArticleCategory)categoryChoice;
        ScientificArticle article = new ScientificArticle(title, authors, dateOfPublication, numberOfPages, category, journal);
        ICollection<ValidationResult> errors = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(article, new ValidationContext(article), errors, validateAllProperties:true);
        if (!isValid)
        {
            throw new ValidationException(String.Join("|", errors.Select(err => err.ErrorMessage)));
        }
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
        ICollection<ValidationResult> errors = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(scientist, new ValidationContext(scientist), errors, validateAllProperties:true);
        if (!isValid)
        {
            throw new ValidationException(String.Join("|", errors.Select(err => err.ErrorMessage)));
        }
        _repository.CreateScientist(scientist);
        return scientist;
    }

    public bool TryParseScientist(string scientistNameString, out Scientist existingScientist)
    {
        if (!(scientistNameString == null || scientistNameString.Trim() == ""))
        {
            foreach (Scientist scientist in GetAllScientists())
            {
                if (_repository.MatchScientistName(scientistNameString, scientist))
                {
                    existingScientist = scientist;
                    return true;
                }
            }
        }
        existingScientist = null;
        return false;
    }

    
}