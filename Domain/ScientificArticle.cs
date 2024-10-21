using System.ComponentModel.DataAnnotations;

namespace ArticleManagement.BL.Domain;

public class ScientificArticle : IValidatableObject
{
    [Key]
    public int ArticleId { get; set; }
    
    [Required] [StringLength(150)]
    public string Title { get; init; }
    public DateOnly DateOfPublication { get; set; }
    public int NumberOfPages { get; set; }
    public ScienceJournal Journal { get; set; }
    public ArticleCategory Category { get; set; }
    public IEnumerable<Scientist> Authors { get; set; }

    public ScientificArticle(string title, IEnumerable<Scientist> authors, DateOnly dateOfPublication, int numberOfPages, ArticleCategory category, ScienceJournal journal = null)
    {
        Title = title;
        Authors = authors;
        DateOfPublication = dateOfPublication;
        NumberOfPages = numberOfPages;
        Category = category;
        Journal = journal;
        SetRelations();
    }

    private void SetRelations()
    {
        RelateAuthors();
        RelateJournal();
    }

    private void RelateAuthors()
    {
        foreach (Scientist author in Authors)
        {
            author.Articles.Add(this);
        }
    }       

    private void RelateJournal()
    {
        Journal?.Articles.Add(this);
    }
    
    private string PrintAuthors()
    {   
        string authorsList = "";
        foreach (Scientist author in Authors)
        {
            authorsList += author.Name + ", ";
        }
        return authorsList.Trim().Trim(',');
        // return String.Join(", ", ArticleAuthors);
    }

    public override string ToString()
    {
        string authors = PrintAuthors();
        
        return $"{Title} ({NumberOfPages} {(NumberOfPages == 1 ? "page" : "pages")}){(authors==""? "" : $"; by {authors}")} [published in \"{(Journal == null? "UNKNOWN" : Journal.JournalName)}\" on {DateOfPublication}] (Article ID: {ArticleId})";
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        List<ValidationResult> errors = new List<ValidationResult>();
        
        // 1ste check
        if (!Enum.IsDefined(Category))
        {
            errors.Add(new ValidationResult("Article category unknown!", new string[] { nameof(Category) }));
        }
        
        return errors;    }
}   