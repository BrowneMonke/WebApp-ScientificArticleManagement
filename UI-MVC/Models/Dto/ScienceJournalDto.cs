using ArticleManagement.BL.Domain;

namespace ArticleManagement.UI.Web.Models.Dto;

public class ScienceJournalDto
{
    public int Id { get; set; }
    public string Name { get; init; }
    public double? Price { get; init; }
    public Country CountryOfOrigin { get; init; }
    
    public ICollection<int> ArticleIds { get; init; }
}