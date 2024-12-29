using ArticleManagement.BL.Domain;

namespace ArticleManagement.UI.CA.Extensions;

public static class ArticleCategoryExtensions
{
    private static readonly string[] NameArray = ["Astrophysics", "Neuroscience", "Marine Ecology", "Quantum Physics", "Electromagnetism", "Chemistry", "Biology"];
    public static string GetString(this ArticleCategory category)
    {
        return NameArray[Array.IndexOf(Enum.GetValues<ArticleCategory>(), category)];
    }   
}