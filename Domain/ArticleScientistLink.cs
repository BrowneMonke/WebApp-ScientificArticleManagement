using System.ComponentModel.DataAnnotations;

namespace ArticleManagement.BL.Domain;

public class ArticleScientistLink
{
    [Required]
    public virtual ScientificArticle Article { get; set; }
    [Required]
    public virtual Scientist Scientist { get; set; }

    public bool IsLeadResearcher { get; set; }
}