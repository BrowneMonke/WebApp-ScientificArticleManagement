using System.ComponentModel.DataAnnotations;

namespace ArticleManagement.BL.Domain;

public class ScientificArticle : IValidatableObject
{
    // scalar-properties
    [Key]
    public int Id { get; set; }
    
    [Required] [StringLength(256, MinimumLength = 5)]
    public string Title { get; init; }
    public DateOnly DateOfPublication { get; init; }
    
    [Range(1,Int32.MaxValue, ErrorMessage = "Enter a valid number of pages!")]
    public int NumberOfPages { get; init; }
    public ArticleCategory Category { get; init; }
    
    // navigation-properties
    public ScienceJournal Journal { get; set; }
    
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
        
        //DoP check
        if (DateOfPublication > DateOnly.FromDateTime(DateTime.Today))
        {
            errors.Add(new ValidationResult("Please enter a valid date. The date may not be in the future.", 
                new string[]{ nameof(DateOfPublication) }));
        }
        
        return errors;    
    }
    
}   