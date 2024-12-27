using ArticleManagement.BL.Domain;

namespace ArticleManagement.UI.Web.Models.Dto;

public class NewScienceJournalDto
{
    public string Name { get; init; }
    public double? Price { get; init; }
    public int YearFounded { get; init; }
    public Country CountryOfOrigin { get; init; }
}