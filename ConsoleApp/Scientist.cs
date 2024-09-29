namespace ConsoleApp;

public class Scientist
{
    public string Name { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public List<ScientificArticle> Articles { get; set; }
    public string Faculty { get; set; }
    public string University { get; set; }

    public Scientist(string name, string faculty, string university, DateOnly? dateOfBirth = null)
    {
        Name = name;
        DateOfBirth = dateOfBirth;
        Articles = new List<ScientificArticle>();
    }
    
    public override string ToString()
    {
        return Name;
    }

    public string GetInfo()
    {
        return $"{Name}, Faculty of {Faculty} at {University} ({(DateOfBirth != null? $"born {DateOfBirth}" : "" )})";
    }
    
}