namespace ArticleManagement.UI.Web.Models.Dto;

public class NewArticleScientistLinkDto
{
    public int ArticleId { get; init; }
    public int ScientistId { get; init; }
    public bool IsLeadResearcher { get; init; }
}