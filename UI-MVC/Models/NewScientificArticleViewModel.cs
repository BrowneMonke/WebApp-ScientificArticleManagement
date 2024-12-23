using System.ComponentModel.DataAnnotations;
using ArticleManagement.BL.Domain;

namespace ArticleManagement.UI.Web.Models;

public class NewScientificArticleViewModel
{
    [Required] [StringLength(256, MinimumLength = 5)]
    public string Title { get; init; }
    [Required(ErrorMessage = "The Date of Publication field is required.")] [CustomValidation(typeof(NewScientificArticleViewModel), nameof(ValidateDateOfPublication))]
    public DateOnly DateOfPublication { get; init; }
    [Required(ErrorMessage = "The Number of Pages field is required.")]
    public int NumberOfPages { get; init; }
    public ArticleCategory Category { get; init; }

    public static ValidationResult ValidateDateOfPublication(DateOnly dateOfPublication)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        if (dateOfPublication > today)
        {
            return new ValidationResult("Only past dates are acceptable as Date of Publication.");
        }

        return ValidationResult.Success;
    }
    
}