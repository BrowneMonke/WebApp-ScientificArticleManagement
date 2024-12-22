using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArticleManagement.BL.Domain;

public class ScientificArticle : IValidatableObject
{
    // scalar-properties
    [Key]
    public int Id { get; set; }
    
    [Required] [StringLength(256, MinimumLength = 5)]
    public string Title { get; init; }
    [Required]
    public DateOnly DateOfPublication { get; set; }
    public int NumberOfPages { get; set; } // TODO: Add validation for date of pub and number of pages (can't be negative)
    public ArticleCategory Category { get; set; }
    
    // navigation-properties
    // [NotMapped]
    public ScienceJournal Journal { get; set; }
    
    // [NotMapped]
    // public ICollection<Scientist> Authors { get; set; }
    public ICollection<ArticleScientistLink> AuthorLinks { get; set; }

    public ScientificArticle(string title)
    {
        Title = title;
        AuthorLinks = [];
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        List<ValidationResult> errors = new List<ValidationResult>();
        
        // Category check
        if (!Enum.IsDefined(Category))
        {
            errors.Add(new ValidationResult("Article category unknown!", new string[] { nameof(Category) }));
        }
        
        return errors;    
    }
    
}   