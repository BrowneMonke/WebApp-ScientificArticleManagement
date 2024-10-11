namespace ConsoleApp;

public class ScientificArticle
{
    public string Title { get; init; }
    public DateOnly DateOfPublication { get; set; }
    public int NumberOfPages { get; set; }
    public ScienceJournal Journal { get; set; }
    public ArticleCategory Category { get; set; }
    public List<Scientist> ArticleAuthors { get; set; }

    public ScientificArticle(string title, List<Scientist> authors, DateOnly dateOfPublication, int numberOfPages, ArticleCategory category, ScienceJournal journal)
    {
        Title = title;
        ArticleAuthors = authors;
        DateOfPublication = dateOfPublication;
        NumberOfPages = numberOfPages;
        Category = category;
        Journal = journal;
        SetRelations();
    }

    private void SetRelations()
    {
        RelateAuthors();
        RelateJournal();
    }

    private void RelateAuthors()
    {
        foreach (Scientist author in ArticleAuthors)
        {
            author.AuthorArticles.Add(this);
        }
    }

    private void RelateJournal()
    {
        Journal.JournalArticles.Add(this);
    }
    
    private string PrintAuthors()
    {   
        string authorsList = "";
        foreach (Scientist author in ArticleAuthors)
        {
            authorsList += author.Name + ", ";
        }
        return authorsList.Trim().Trim(',');
        // return String.Join(", ", ArticleAuthors);
    }

    public override string ToString()
    {
        return $"{Title} ({NumberOfPages} pages); by {PrintAuthors()} [published in \"{Journal.JournalName}\" on {DateOfPublication}]";
    }
}