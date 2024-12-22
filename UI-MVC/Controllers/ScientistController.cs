using ArticleManagement.BL;
using Microsoft.AspNetCore.Mvc;

namespace ArticleManagement.UI.Web.Controllers;

public class ScientistController : Controller
{
    private readonly IManager _manager;

    public ScientistController(IManager manager)
    {
        _manager = manager;
    }
    
    public ViewResult Index()
    {
        var scientists = _manager.GetAllScientists();
        return View(scientists);
    }
    
    public ViewResult Details(int id)
    {
        var scientist = _manager.GetScientist(id);
        return View(scientist);
    }
    
    
}