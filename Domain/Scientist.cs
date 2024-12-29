using System.ComponentModel.DataAnnotations;

namespace ArticleManagement.BL.Domain;

public class Scientist : IValidatableObject
{
    [Key]
    public int Id { get; set; }
    
    [Required] [MaxLength(50)]
    public string Name { get; init; }
    public DateOnly? DateOfBirth { get; init; }

    [Required]
    public string Faculty { get; init; }

    [Required]
    public string University { get; init; }

    // navigation-property
    public ICollection<ArticleScientistLink> ArticleLinks { get; set; }

    public Scientist(string name, string faculty, string university, DateOnly? dateOfBirth = null)
    {
        Name = name;
        Faculty = faculty;
        University = university;
        DateOfBirth = dateOfBirth;
        ArticleLinks = new List<ArticleScientistLink>();
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        List<ValidationResult> errors = [];
        
        //DoB check
        if (DateOfBirth > DateOnly.FromDateTime(DateTime.Today))
        {
            errors.Add(new ValidationResult("Please enter a valid Date of Birth! The date may not be in the future.", 
                new string[]{ nameof(DateOfBirth) }));
        }

        return errors;   
    }
}