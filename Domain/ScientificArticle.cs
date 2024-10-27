using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArticleManagement.BL.Domain;

public class ScientificArticle : IValidatableObject
{
    private ScienceJournal _journal;
    private IEnumerable<Scientist> _authors;
    
    // scalar-properties
    [Key]
    public int ArticleId { get; set; }
    
    [Required] [StringLength(150)]
    public string Title { get; init; }
    public DateOnly DateOfPublication { get; set; }
    public int NumberOfPages { get; set; }
    public ArticleCategory Category { get; set; }
    
    // navigation-properties
    [NotMapped]
    public ScienceJournal Journal
    {
        get => _journal;
        set
        {
            _journal = value;
            RelateJournal();
        }
    }
    
    [NotMapped]
    public IEnumerable<Scientist> Authors
    {
        get => _authors;
        set
        {
            _authors = value;
            RelateAuthors();
        }
    }

    public ScientificArticle(string title)
    {
        Title = title;
        Authors = [];
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
        string authorNamesString = "";
        foreach (Scientist author in Authors)
        {
            authorNamesString += author.Name + ", ";
        }
        return authorNamesString.Trim().Trim(',');
    }

    public override string ToString()
    {
        string authors = PrintAuthors();
        
        return $"{Title} ({NumberOfPages} {(NumberOfPages == 1 ? "page" : "pages")}){(authors==""? "" : $"; by {authors}")} [published in \"{(Journal == null? "UNKNOWN" : Journal.JournalName)}\" on {DateOfPublication}] (Article ID: {ArticleId})";
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        List<ValidationResult> errors = new List<ValidationResult>();
        
        if (!Enum.IsDefined(Category))
        {
            errors.Add(new ValidationResult("Article category unknown!", new string[] { nameof(Category) }));
        }
        
        return errors;    
    }
    
}   