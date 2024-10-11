namespace ConsoleApp;

public class Scientist
{
    public string Name { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public List<ScientificArticle> AuthorArticles { get; set; }
    public string Faculty { get; set; }
    public string University { get; set; }

    public Scientist(string name, string faculty, string university, DateOnly? dateOfBirth = null)
    {
        Name = name;
        Faculty = faculty;
        University = university;
        DateOfBirth = dateOfBirth;
        AuthorArticles = new List<ScientificArticle>();
    }
    
    public override string ToString()
    {
        return $"{Name}, Faculty of {Faculty} at {University} {(DateOfBirth != null? $"(born {DateOfBirth})" : "" )}";
    }


}