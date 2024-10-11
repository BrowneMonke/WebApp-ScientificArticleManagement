namespace ArticleManagement.BL.Domain;

public class ScientificArticle
{
    public int ArticleId { get; set; }
    public string Title { get; init; }
    public DateOnly DateOfPublication { get; set; }
    public int NumberOfPages { get; set; }
    public ScienceJournal Journal { get; set; }
    public ArticleCategory Category { get; set; }
    public ICollection<Scientist> Authors { get; set; }

    public ScientificArticle(string title, ICollection<Scientist> authors, DateOnly dateOfPublication, int numberOfPages, ArticleCategory category, ScienceJournal journal)
    {
        Title = title;
        Authors = authors;
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
        foreach (Scientist author in Authors)
        {
            author.Articles.Add(this);
        }
    }       

    private void RelateJournal()
    {
        Journal.Articles.Add(this);
    }
    
    private string PrintAuthors()
    {   
        string authorsList = "";
        foreach (Scientist author in Authors)
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