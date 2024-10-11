namespace ArticleManagement.BL.Domain;

public class ScienceJournal
{
    public int JournalId { get; set; }
    public string JournalName { get; set; }
    public ICollection<ScientificArticle> Articles { get; set; }
    public double? Price { get; set; }

    public ScienceJournal(string name, double? price = null)
    {
        JournalName = name;
        Price = price;
        Articles = new List<ScientificArticle>();
    }

    public override string ToString()
    {
        return $"{JournalName}{(Price != null ? $" [${Price}]" : "")}";
    }
}