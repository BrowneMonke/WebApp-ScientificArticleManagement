using ArticleManagement.BL;
using Microsoft.AspNetCore.Mvc;

namespace ArticleManagement.UI.Web.Controllers;

public class ScienceJournalController : Controller
{
    private readonly IManager _manager;

    public ScienceJournalController(IManager manager)
    {
        _manager = manager;
    }
    
    public ViewResult Index()
    {
        return View();
    }
    
}