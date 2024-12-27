using ArticleManagement.BL;
using ArticleManagement.BL.Domain;
using Microsoft.AspNetCore.Mvc;

namespace ArticleManagement.UI.Web.Controllers.Api;

[ApiController] [Route("/api/[controller]")]
public class ScientificArticlesController : ControllerBase
{
    private readonly IManager _manager;

    public ScientificArticlesController(IManager manager)
    {
        _manager = manager;
    }

    [HttpGet]
    public IActionResult GetArticlesOfScientist(int scientistId)
    {
        if (_manager.GetScientist(scientistId) is null) return NotFound(); // 404
        
        var articlesOfScientist =  _manager.GetArticlesOfScientist(scientistId);
        
        if (articlesOfScientist is null || !articlesOfScientist.Any()) 
            return NoContent(); // 204

        return Ok(articlesOfScientist); // 200
    }
    
}