using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArticleManagement.BL.Domain;

public class Scientist //: IValidatableObject
{
    [Key]
    public int Id { get; set; }
    
    [Required] [MaxLength(50)]
    public string Name { get; set; }
    public DateOnly? DateOfBirth { get; set; }

    [Required]
    public string Faculty { get; set; }

    [Required]
    public string University { get; set; }

    // navigation-property
    // [NotMapped]
    // public ICollection<ScientificArticle> Articles { get; set; }
    public ICollection<ArticleScientistLink> ArticleLinks { get; set; }

    public Scientist(string name, string faculty, string university, DateOnly? dateOfBirth = null)
    {
        Name = name;
        Faculty = faculty;
        University = university;
        DateOfBirth = dateOfBirth;
        ArticleLinks = new List<ArticleScientistLink>();
    }

    /*public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        List<ValidationResult> errors = new List<ValidationResult>();
        
        // 1ste check
        if (!)
        {
            errors.Add(new ValidationResult("Article category unknown!", new string[] { nameof(Category) }));
        }
    }*/
}