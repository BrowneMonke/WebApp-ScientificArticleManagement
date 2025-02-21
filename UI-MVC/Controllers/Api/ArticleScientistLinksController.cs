using System.ComponentModel.DataAnnotations;
using ArticleManagement.BL;
using ArticleManagement.BL.Domain;
using ArticleManagement.UI.Web.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArticleManagement.UI.Web.Controllers.Api;

[ApiController] [Route("/api/[controller]")]
public class ArticleScientistLinksController : ControllerBase
{
    private readonly IManager _manager;

    public ArticleScientistLinksController(IManager manager)
    {
        _manager = manager;
    }

    [HttpGet]
    public IActionResult GetLink(int articleId, int scientistId)
    {
        var asLk = _manager.GetArticleScientistLink(articleId, scientistId);
        
        if (asLk is null) return NotFound();
        
        return Ok(asLk);
    }
    
    
    [HttpPost] [Authorize]
    public IActionResult PostLink(NewArticleScientistLinkDto newArticleScientistLinkDto)
    {
        ArticleScientistLink asLk;
        try
        {
            asLk = _manager.AddArticleScientistLink(newArticleScientistLinkDto.ArticleId,
                newArticleScientistLinkDto.ScientistId, newArticleScientistLinkDto.IsLeadResearcher);
        }
        catch (ValidationException exception)
        {
            return Conflict(exception.Message);
        }
        
        if (asLk is null) return BadRequest();

        return CreatedAtAction("GetLink", new { articleId = asLk.Article.Id, scientistId = asLk.Scientist.Id }, asLk);
    }
    
}