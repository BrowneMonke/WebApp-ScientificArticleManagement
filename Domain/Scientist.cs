using System.ComponentModel.DataAnnotations;

namespace ArticleManagement.BL.Domain;

public class Scientist
{
    [Key] [Range(1, Int32.MaxValue)]
    public int ScientistId { get; set; }
    
    [Required] [StringLength(50)]
    public string Name { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public ICollection<ScientificArticle> Articles { get; set; }
    public string Faculty { get; set; }
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


}