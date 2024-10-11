namespace ArticleManagement.BL.Domain.Extensions;

public static class ArticleCategoryExtensions
{
    private static readonly string[] NameArray = ["Astrophysics", "Neuroscience", "Marine Ecology", "Biology", "Chemistry"];
    public static string GetString(this ArticleCategory category)
    {
        return NameArray[Array.IndexOf(Enum.GetValues<ArticleCategory>(), category)];
    }   
}