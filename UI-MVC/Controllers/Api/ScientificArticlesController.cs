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

    [HttpGet("{scientistId}")]
    public IActionResult GetArticlesByScientist(int scientistId)
    {
        if (_manager.GetScientist(scientistId) is null) return NotFound(); // 404
        
        var articlesByScientist =  _manager.GetArticlesByScientist(scientistId);
        
        if (articlesByScientist is null || !articlesByScientist.Any()) 
            return NoContent(); // 204

        return Ok(articlesByScientist); // 200
    }
    
    [HttpGet("articles/not-by/{scientistId}")]
    public IActionResult GetArticlesNotByScientist(int scientistId)
    {
        if (_manager.GetScientist(scientistId) is null) return NotFound(); // 404
        
        var articlesNotByScientist =  _manager.GetArticlesNotByScientist(scientistId);
        
        if (articlesNotByScientist is null || !articlesNotByScientist.Any()) 
            return NoContent(); // 204

        return Ok(articlesNotByScientist); // 200
    }

    [HttpGet]
    public IActionResult GetAllArticles()
    {
        var articles = _manager.GetAllArticles();
        if (articles is null || !articles.Any()) 
            return NoContent(); // 204

        return Ok(articles);
    }
    
}