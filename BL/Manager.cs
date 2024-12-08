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

    public IEnumerable<ScientificArticle> GetArticlesByCategory(ArticleCategory categoryChoice)
    {
        return _repository.ReadArticlesByCategory(categoryChoice);
    }
    public IEnumerable<ScientificArticle> GetArticlesByCategoryWithAuthorsAndJournals(ArticleCategory categoryChoice)
    {
        return _repository.ReadArticlesByCategoryWithAuthorsAndJournals(categoryChoice);
    }

    public IEnumerable<ScientificArticle> GetArticlesOfScientist(int scientistId)
    {
        return _repository.ReadArticlesOfScientist(scientistId);
    }

    public ScientificArticle GetArticle(int articleId)
    {
        return _repository.ReadArticle(articleId);
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

    public ScientificArticle AddArticle(string title, IEnumerable<Scientist> authors, DateOnly dateOfPublication, int numberOfPages, ArticleCategory categoryChoice, ScienceJournal journal)
    {
        List<ArticleScientistLink> linkArticleScientistsList = [];
        int leadResearcherToggle = 0;
        foreach (Scientist scientist in authors)
        {
            ArticleScientistLink authorInstance = new ArticleScientistLink
            {
                Scientist = scientist,
                IsLeadResearcher = (leadResearcherToggle == 0)
            };
            linkArticleScientistsList.Add(authorInstance);
            leadResearcherToggle++;
        }

        ScientificArticle article = new ScientificArticle(title)
        {
            AuthorLinks = linkArticleScientistsList,
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

        RelateAuthors(article);
        RelateJournal(article);
        _repository.CreateArticle(article);

        return article;
    }

    public IEnumerable<Scientist> GetAllScientists()
    {
        return _repository.ReadAllScientists();
    }

    public IEnumerable<Scientist> GetAllScientistsWithArticles()
    {
        return _repository.ReadAllScientistsWithArticles();
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

    private void CheckLinkValidity(int articleId, int scientistId)
    {
        if (articleId == Int32.MaxValue || scientistId == Int32.MaxValue || _repository.ReadArticle(articleId) == null || _repository.ReadScientist(scientistId) == null)
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
            Article = _repository.ReadArticle(articleId),
            Scientist = _repository.ReadScientist(scientistId),
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
            throw new ValidationException("Invalid ID values!");
        }
        _repository.DeleteArticleScientistLink(articleId, scientistId);
    }
}