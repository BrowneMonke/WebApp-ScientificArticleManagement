namespace ConsoleApp;

public class ScienceJournal
{
    public string JournalName { get; set; }
    public List<ScientificArticle> Articles { get; set; }
    public double? Price { get; set; }

    public ScienceJournal(string name, double? price = null)
    {
        JournalName = name;
        Price = price;
        Articles = new List<ScientificArticle>();
    }

    public override string ToString()
    {
        return $"{JournalName} [${Price}]";
    }
}