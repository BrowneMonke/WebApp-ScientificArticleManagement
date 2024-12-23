using ArticleManagement.BL;
using ArticleManagement.BL.Domain;
using ArticleManagement.UI.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace ArticleManagement.UI.Web.Controllers;

// /ScientificArticle/<actionMethod>
public class ScientificArticleController : Controller
{
    private readonly IManager _manager;

    public ScientificArticleController(IManager manager)
    {
        _manager = manager;
    }

    public ViewResult Index()
    {
        var articles = _manager.GetAllArticles();
        return View(articles);
    }
    
    [HttpGet]
    public ViewResult Add()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Add(NewScientificArticleViewModel newArticle)
    {
        if (!ModelState.IsValid)
        {
            return View(newArticle);
        }

        var article = new ScientificArticle(newArticle.Title)
        {
            DateOfPublication = newArticle.DateOfPublication,
            NumberOfPages = newArticle.NumberOfPages,
            Category = newArticle.Category
        };
        var addedArticle = _manager.AddArticle(article);
        
        return RedirectToAction("Details", new{id = addedArticle.Id});
    }
    
    public ViewResult Details(int id)
    {
        var article = _manager.GetArticleByIdWithAuthorsAndJournal(id);
        return View(article);
    }
    
}