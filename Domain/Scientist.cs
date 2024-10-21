using System.ComponentModel.DataAnnotations;

namespace ArticleManagement.BL.Domain;

public class Scientist //: IValidatableObject
{
    [Key]
    public int ScientistId { get; set; }
    
    [Required] [MaxLength(50)]
    public string Name { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public ICollection<ScientificArticle> Articles { get; set; }
    
    [Required]
    public string Faculty { get; set; }
    
    [Required]
    public string University { get; set; }

    public Scientist(string name, string faculty, string university, DateOnly? dateOfBirth = null)
    {
        Name = name;
        Faculty = faculty;
        University = university;
        DateOfBirth = dateOfBirth;
        Articles = new List<ScientificArticle>();
    }
    

    public override string ToString()
    {
        return $"{Name} (ID: {ScientistId}), Faculty of {Faculty} at {University} {(DateOfBirth != null? $"(born {DateOfBirth})" : "" )}";
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