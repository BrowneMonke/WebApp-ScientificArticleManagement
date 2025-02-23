using ArticleManagement.BL.Domain;

namespace ArticleManagement.UI.Web.Models.Dto;

public class UpdateScientificArticleDto
{
    public int Id { get; set; }
    public ArticleCategory Category { get; set; }
}