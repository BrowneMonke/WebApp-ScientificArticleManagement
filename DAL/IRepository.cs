using ArticleManagement.BL.Domain;

namespace ArticleManagement.DAL;

public interface IRepository
{
    IEnumerable<ScientificArticle> ReadAllArticles();
    IEnumerable<ScientificArticle> ReadArticlesByCategory(ArticleCategory categoryChoice);
    ScientificArticle ReadArticle(int id);
    void CreateArticle(ScientificArticle articleToInsert);

    IEnumerable<Scientist> ReadAllScientists();
    bool MatchScientistName(string nameString, Scientist scientist);
    IEnumerable<Scientist> ReadScientistsByNameAndDateOfBirth(string nameString, string dobString);
    Scientist ReadScientist(int id);
    void CreateScientist(Scientist scientistToInsert);
}