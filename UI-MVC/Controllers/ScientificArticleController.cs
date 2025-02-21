using ArticleManagement.BL;
using ArticleManagement.BL.Domain;
using ArticleManagement.UI.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArticleManagement.UI.Web.Controllers;

// /ScientificArticle/<actionMethod>
[Authorize]
public class ScientificArticleController : Controller
{
    private readonly IManager _manager;

    public ScientificArticleController(IManager manager)
    {
        _manager = manager;
    }

    [AllowAnonymous]
    public ViewResult Index()
    {
        var articles = _manager.GetAllArticles();
        return View(articles);
    }

    [AllowAnonymous]
    public ViewResult Details(int id)
    {
        var article = _manager.GetArticleByIdWithAuthorsAndJournalAndDataOwner(id);
        return View(article);
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

        string currentUserName = User.Identity?.Name;
        var addedArticle = _manager.AddArticle(newArticle.Title, new List<Scientist>(), newArticle.DateOfPublication,
            newArticle.NumberOfPages, newArticle.Category, currentUserName);
        
        return RedirectToAction("Details", new{id = addedArticle.Id});
    }
}