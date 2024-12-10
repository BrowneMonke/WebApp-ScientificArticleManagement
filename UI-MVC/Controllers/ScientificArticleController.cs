using ArticleManagement.BL;
using ArticleManagement.BL.Domain;
using ArticleManagement.UI.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArticleManagement.UI.Web.Controllers;

public class ScientificArticleController
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
    
}