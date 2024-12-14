using ArticleManagement.BL;
using ArticleManagement.BL.Domain;
using ArticleManagement.UI.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArticleManagement.UI.Web.Controllers;

// /ScientificArticle/<actionmethod>
public class ScientificArticleController : Controller
{
    private readonly IManager _manager;

    public ScientificArticleController(IManager manager)
    {
        _manager = manager;
    }
    
    // GET: /Book/Index
    /*public string Index()
    {
        var articles = _manager.GetAllArticles();
        return $"Hello world! Aantal artikelen: {articles.Count()}";
    }*/

    public ViewResult Index()
    {
        var articles = _manager.GetAllArticles();
        return View(articles);
    }
    public void Add()
    {
        throw new NotImplementedException();
    }
    
    public ViewResult Details(int id)
    {
        var article = _manager.GetArticleByIdWithAuthorsAndJournal(id);
        return View(article);
    }
    
}