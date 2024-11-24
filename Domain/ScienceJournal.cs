using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArticleManagement.BL.Domain;

public class ScienceJournal
{
    // scalar-properties
    [Key]
    public int JournalId { get; set; }
    public string JournalName { get; set; }
    public double? Price { get; set; }
    public bool HasValue { get; set; }
    
    // navigation-property
    [NotMapped]
    public virtual ICollection<ScientificArticle> Articles { get; set; }

    /*private ScienceJournal()
    {
        // EF needs this constructor even though it is never called by 
        // my code in the application. EF needs it to set up the contexts

        // Failure to have it will result in a 
        //  No suitable constructor found for entity type 'ScienceJournal' exception   
    }*/

    public ScienceJournal(string journalName, double? price = null)
    {
        JournalName = journalName;
        Articles = new List<ScientificArticle>();
        Price = price;
        if (Price > 0)
            HasValue = true;
        else 
            Price = null;
    }

    /*public override string ToString()
    {
        return $"{JournalName}{(HasValue ? $" [${Price}]" : "")}";
    }*/
}