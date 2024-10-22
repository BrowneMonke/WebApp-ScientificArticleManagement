using ArticleManagement.BL.Domain;

namespace ArticleManagement.DAL.EF;

public class EFRepository : IRepository
    
{
    public IEnumerable<ScientificArticle> ReadAllArticles()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<ScientificArticle> ReadArticlesByCategory(ArticleCategory categoryChoice)
    {
        throw new NotImplementedException();
    }

    public ScientificArticle ReadArticle(int id)
    {
        throw new NotImplementedException();
    }

    public void CreateArticle(ScientificArticle articleToInsert)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Scientist> ReadAllScientists()
    {
        throw new NotImplementedException();
    }

    public bool MatchScientistName(string nameString, Scientist scientist)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Scientist> ReadScientistsByNameAndDateOfBirth(string nameString, string dobString)
    {
        throw new NotImplementedException();
    }

    public Scientist ReadScientist(int id)
    {
        throw new NotImplementedException();
    }

    public void CreateScientist(Scientist scientistToInsert)
    {
        throw new NotImplementedException();
    }
}