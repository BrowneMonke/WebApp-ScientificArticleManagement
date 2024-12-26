using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArticleManagement.BL.Domain;

public class ScienceJournal
{
    //TODO: Minstens 5 instanties door de dataseeder laten aangemaakt worden (en die dan ook koppelen aan article instanties!!)
    // scalar-properties
    [Key]
    public int Id { get; set; }
    public string Name { get; private set; }
    public double? Price { get; init; }
    public int YearFounded { get; init; }
    public Country CountryOfOrigin { get; init; }
    
    // navigation-property
    [NotMapped]
    public ICollection<ScientificArticle> Articles { get; set; }

    public ScienceJournal(string name, double? price = null)
    {
        Name = name;
        Articles = new List<ScientificArticle>();
        if (price > 0)
            Price = price;
    }

}