using System.ComponentModel.DataAnnotations;

namespace ArticleManagement.BL.Domain;

public class ScienceJournal : IValidatableObject
{
    // scalar-properties
    [Key]
    public int Id { get; set; }
    public string Name { get; private set; }
    public double? Price { get; init; }
    public int YearFounded { get; init; }
    public Country CountryOfOrigin { get; init; }
    
    // navigation-property
    public ICollection<ScientificArticle> Articles { get; set; }

    public ScienceJournal(string name, double? price = null)
    {
        Name = name;
        Articles = new List<ScientificArticle>();
        if (price > 0)
            Price = price;
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        List<ValidationResult> errors = [];
        
        // Country check
        if (!Enum.IsDefined(CountryOfOrigin))
        {
            errors.Add(new ValidationResult("Country of origin unknown!", new string[] { nameof(CountryOfOrigin) }));
        }
        //Year check
        if (YearFounded < 0 || YearFounded > DateTime.Today.Year)
        {
            errors.Add(new ValidationResult("The year can neither be negative nor be in the future!", new[]{ nameof(YearFounded)}));
        }
        return errors;
    }
}