using ArticleManagement.BL;
using ArticleManagement.BL.Domain;
using ArticleManagement.UI.Web.Models.Dto;
using Microsoft.AspNetCore.Authorization;
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

    [HttpGet("by-scientist/{scientistId}")]
    public IActionResult GetArticlesByScientist(int scientistId)
    {
        if (_manager.GetScientistById(scientistId) is null) return NotFound(); // 404
        
        var articlesByScientist =  _manager.GetArticlesByScientist(scientistId);
        
        if (articlesByScientist is null || !articlesByScientist.Any()) 
            return NoContent(); // 204

        return Ok(articlesByScientist); // 200
    }
    
    [HttpGet("not-by-scientist/{scientistId}")]
    public IActionResult GetArticlesNotByScientist(int scientistId)
    {
        if (_manager.GetScientistById(scientistId) is null) return NotFound(); // 404
        
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
    
    [Authorize][HttpPut]
    public IActionResult PutArticle(UpdateScientificArticleDto updateScientificArticleDto)
    {
        ScientificArticle existingArticle = _manager.GetArticleByIdWithDataOwner(updateScientificArticleDto.Id);
        if (existingArticle == null) return NotFound(); // 404

        if (!(existingArticle.DataOwner.UserName == User.Identity?.Name || User.IsInRole("Admin")))
        {
            return Forbid();
        }

        var updatedArticle = _manager.ChangeArticle(existingArticle, updateScientificArticleDto.Category);
        return Ok(updatedArticle);
    }
    
}