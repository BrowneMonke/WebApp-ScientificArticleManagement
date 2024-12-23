using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArticleManagement.BL.Domain;

public class ScienceJournal
{
    // scalar-properties
    [Key]
    public int Id { get; set; }
    public string JournalName { get; init; }
    public double? Price { get; init; }
    public bool HasPrice { get; private set; }
    
    // navigation-property
    [NotMapped]
    public ICollection<ScientificArticle> Articles { get; set; }

    public ScienceJournal(string journalName, double? price = null)
    {
        JournalName = journalName;
        Articles = new List<ScientificArticle>();
        Price = price;
        if (Price > 0)
            HasPrice = true;
        else 
            Price = null;
    }

    /*public override string ToString()
    {
        return $"{JournalName}{(HasPrice ? $" [${Price}]" : "")}";
    }*/
}