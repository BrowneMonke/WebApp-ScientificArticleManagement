using System.ComponentModel.DataAnnotations;

namespace ArticleManagement.BL.Domain;

public class ArticleScientist
{
    [Required]
    public ScientificArticle Article { get; set; }
    [Required]
    public Scientist Scientist { get; set; }

    public int ContributionOrder { get; set; }
    public bool IsLeadResearcher { get; set; }
}