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

    public IEnumerable<ScientificArticle> GetAllArticlesWithAuthorsAndJournals()
    {
        return _repository.ReadAllArticlesWithAuthorsAndJournals();
    }
    
    public IEnumerable<ScientificArticle> GetArticlesByCategoryWithAuthorsAndJournals(int categoryChoice)
    {
        if (!Enum.IsDefined((ArticleCategory)categoryChoice))
        {
            throw new ValidationException("Invalid Category Number! Please try again.");
        }
        return _repository.ReadArticlesByCategoryWithAuthorsAndJournals((ArticleCategory)categoryChoice);
    }

    public IEnumerable<ScientificArticle> GetArticlesByScientist(int scientistId)
    {
        return _repository.ReadArticlesByScientist(scientistId);
    }

    public IEnumerable<ScientificArticle> GetArticlesNotByScientist(int scientistId)
    {
        return _repository.ReadArticlesNotByScientist(scientistId);
    }
    
    public ScientificArticle GetArticleByIdWithDataOwner(int articleId)
    {
        return _repository.ReadArticleByIdWithDataOwner(articleId);
    }
    public ScientificArticle GetArticleByIdWithAuthorsAndJournal(int articleId)
    {
        return _repository.ReadArticleByIdWithAuthorsAndJournal(articleId);
    }
    
    public ScientificArticle GetArticleByIdWithAuthorsAndJournalAndDataOwner(int articleId)
    {
        return _repository.ReadArticleByIdWithAuthorsAndJournalAndDataOwner(articleId);
    }
    
    private static void RelateAuthors(ScientificArticle article)
    {
        foreach (ArticleScientistLink linkArticleScientist in article.AuthorLinks)
        {
            linkArticleScientist.Article = article;
            linkArticleScientist.Scientist.ArticleLinks.Add(linkArticleScientist);
        }
    }       

    private static void RelateJournal(ScientificArticle article)
    {
        article.Journal?.Articles.Add(article);
    }

    private void FillAuthorsList(IEnumerable<Scientist> authors,
        List<ArticleScientistLink> articleScientistLinksList)
    {
        int leadResearcherToggle = 0;
        foreach (Scientist scientist in authors)
        {
            ArticleScientistLink authorInstance = new ArticleScientistLink
            {
                Scientist = scientist,
                IsLeadResearcher = (leadResearcherToggle == 0)
            };
            articleScientistLinksList.Add(authorInstance);
            leadResearcherToggle++;
        }
    }

    private void ValidateArticle(ScientificArticle article)
    {
        ICollection<ValidationResult> errors = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(article, new ValidationContext(article), errors, validateAllProperties:true);
        if (!isValid)
        {
            throw new ValidationException(String.Join("|", errors.Select(err => err.ErrorMessage)));
        }
    }
    
    private void ValidateAndAddArticle(ScientificArticle article)
    {
        ValidateArticle(article);
        RelateAuthors(article);
        RelateJournal(article);
        _repository.CreateArticle(article);
    }
    
    public ScientificArticle AddArticle(string title, IEnumerable<Scientist> authors, DateOnly dateOfPublication, int numberOfPages, ArticleCategory categoryChoice, string userName, ScienceJournal journal = null)
    {
        List<ArticleScientistLink> articleScientistLinksList = [];
        FillAuthorsList(authors, articleScientistLinksList);

        ScientificArticle article = new ScientificArticle(title)
        {
            AuthorLinks = articleScientistLinksList,
            DateOfPublication = dateOfPublication,
            NumberOfPages = numberOfPages,
            Category = categoryChoice,
            Journal = journal,
            DataOwner = _repository.GetUserByUserName(userName)
        };
        
        ValidateAndAddArticle(article);

        return article;
    }

    public ScientificArticle ChangeArticle(ScientificArticle existingArticle, ArticleCategory updatedCategory)
    {
        existingArticle.Category = updatedCategory;
        ValidateArticle(existingArticle);
        _repository.UpdateArticle(existingArticle);
        return existingArticle;
    }

    public IEnumerable<Scientist> GetAllScientists()
    {
        return _repository.ReadAllScientists();
    }

    public IEnumerable<Scientist> GetAllScientistsWithArticles()
    {
        return _repository.ReadAllScientistsWithArticles();
    }

    public IEnumerable<Scientist> GetScientistsByNameAndDateOfBirthWithArticles(string nameString, DateOnly? dateOfBirth)
    {
        if (nameString.Trim() == "" && dateOfBirth == null) return _repository.ReadAllScientistsWithArticles();
        return _repository.ReadScientistsByNameAndDateOfBirthWithArticles(nameString, dateOfBirth);
    }

    public Scientist GetScientistById(int scientistId)
    {
        return _repository.ReadScientistById(scientistId);
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


    public ArticleScientistLink GetArticleScientistLink(int articleId, int scientistId)
    {
        return _repository.ReadArticleScientistLinkByArticleIdAndScientistId(articleId, scientistId);
    }
    
    private void CheckLinkValidity(int articleId, int scientistId)
    {
        if (articleId == Int32.MaxValue || scientistId == Int32.MaxValue || _repository.ReadArticleById(articleId) == null || _repository.ReadScientistById(scientistId) == null)
        {
            throw new ValidationException("Invalid ID values.\nPlease try again with valid values.");
        }
        ArticleScientistLink articleScientistLinkToAdd = _repository.ReadArticleScientistLinkByArticleIdAndScientistId(articleId, scientistId);
        if (articleScientistLinkToAdd != null)
        {
            throw new ValidationException("Article-Scientist relation already exists!");
        }
    }
    
    public ArticleScientistLink AddArticleScientistLink(int articleId, int scientistId, bool isLeadResearcher = false)
    {
        CheckLinkValidity(articleId, scientistId);
        if (isLeadResearcher)
        {
            foreach (var asLink in _repository.ReadArticleScientistLinksByArticleId(articleId))
            {
                asLink.IsLeadResearcher = false;
            }
        }
        ArticleScientistLink articleScientistLinkToAdd = new ArticleScientistLink
        {
            Article = _repository.ReadArticleById(articleId),
            Scientist = _repository.ReadScientistById(scientistId),
            IsLeadResearcher = isLeadResearcher
        };
        _repository.CreateArticleScientistLink(articleScientistLinkToAdd);
        return articleScientistLinkToAdd;
    }

    public void RemoveArticleScientistLink(int articleId, int scientistId)
    {
        ArticleScientistLink articleScientistLink =
            _repository.ReadArticleScientistLinkByArticleIdAndScientistId(articleId, scientistId);
        if (articleScientistLink == null)
        {
            throw new ValidationException("Invalid ID values!\nPlease try again with valid values.");
        }
        _repository.DeleteArticleScientistLink(articleScientistLink);
    }

    public IEnumerable<ScienceJournal> GetAllJournals()
    {
        return _repository.ReadAllJournals();
    }

    public ScienceJournal GetJournalByIdWithArticles(int id)
    {
        return _repository.ReadJournalByIdWithArticles(id);
    }

    public ScienceJournal AddJournal(String name, int yearFounded, Country countryOfOrigin, double? price = null)
    {
        var journal = new ScienceJournal(name, price)
        {
            YearFounded = yearFounded,
            CountryOfOrigin = countryOfOrigin
        };
        
        List<ValidationResult> errors = [];
        var journalIsValid = Validator.TryValidateObject(journal, new ValidationContext(journal), errors, true);
        if (!journalIsValid)
        {
            throw new ValidationException(String.Join('|', errors.Select(error => error.ErrorMessage)));
        }
        
        _repository.CreateJournal(journal);

        return journal;
    }
}