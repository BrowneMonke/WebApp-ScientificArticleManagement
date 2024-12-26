using ArticleManagement.BL;
using ArticleManagement.BL.Domain;
using ArticleManagement.UI.Web.Models;
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
        var journals = _manager.GetAllJournals();
        return View(journals);
    }
    
}