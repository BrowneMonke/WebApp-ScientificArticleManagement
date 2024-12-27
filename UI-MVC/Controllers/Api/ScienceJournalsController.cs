using ArticleManagement.BL;
using ArticleManagement.BL.Domain;
using ArticleManagement.UI.Web.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace ArticleManagement.UI.Web.Controllers.Api;

[ApiController] [Route("/api/[controller]")]
public class ScienceJournalsController : ControllerBase
{
    private readonly IManager _manager;

    public ScienceJournalsController(IManager manager)
    {
        _manager = manager;
    }

    [HttpGet]
    public IActionResult GetJournals()
    {
        IEnumerable<ScienceJournal> journals = _manager.GetAllJournals();

        if (!journals.Any()) return NoContent(); // 204
        
        return Ok(journals); // 200
    }
    
    [HttpGet("{id}")]
    public IActionResult GetJournal(int id)
    {
        ScienceJournal journal = _manager.GetJournalByIdWithArticles(id);

        if (journal is null) return NotFound(); // 404

        ScienceJournalDto journalDto = new ScienceJournalDto()
        {
            Id = journal.Id,
            Name = journal.Name,
            Price = journal.Price,
            YearFounded = journal.YearFounded,
            CountryOfOrigin = journal.CountryOfOrigin,
            ArticleIds = new List<int>()
        };
        foreach (var article in journal.Articles)
        {
            journalDto.ArticleIds.Add(article.Id);
        }
        
        return Ok(journalDto); // 200
    }

    [HttpPost]
    public IActionResult PostJournal(NewScienceJournalDto newScienceJournalDto)
    {
        ScienceJournal createdJournal = _manager.AddJournal(newScienceJournalDto.Name, newScienceJournalDto.YearFounded,
            newScienceJournalDto.CountryOfOrigin, newScienceJournalDto.Price);

        return CreatedAtAction("GetJournal", new { id = createdJournal.Id }, createdJournal);
    }
    
}