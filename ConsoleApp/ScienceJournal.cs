namespace ConsoleApp;

public class ScienceJournal
{
    public string JournalName { get; set; }
    public List<ScientificArticle> JournalArticles { get; set; }
    public double? Price { get; set; }

    public ScienceJournal(string name, double? price = null) // TODO: ask teacher about primary constructor
    {
        JournalName = name;
        Price = price;
        JournalArticles = new List<ScientificArticle>();
    }

    public override string ToString()
    {
        return $"{JournalName}{(Price != null ? $" [${Price}]" : "")}";
    }
}