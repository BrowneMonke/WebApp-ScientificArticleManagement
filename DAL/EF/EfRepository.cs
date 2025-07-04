﻿using ArticleManagement.BL.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ArticleManagement.DAL.EF;

public class EfRepository : IRepository
{
    private readonly ArticleDbContext _articleDbContext;

    public EfRepository(ArticleDbContext articleDbContext)
    {
        _articleDbContext = articleDbContext;
    }

    public IEnumerable<ScientificArticle> ReadAllArticles()
    {
        return _articleDbContext.Articles.ToList();
    }

    public IEnumerable<ScientificArticle> ReadAllArticlesWithAuthorsAndJournals()
    {
        IQueryable<ScientificArticle> articles = _articleDbContext.Articles
                                                                    .Include(art => art.Journal)
                                                                    .Include(art => art.AuthorLinks)
                                                                    .ThenInclude(authLk => authLk.Scientist);
        return articles.ToList();
    }
    
    public IEnumerable<ScientificArticle> ReadArticlesByCategoryWithAuthorsAndJournals(ArticleCategory categoryChoice)
    {
        IQueryable<ScientificArticle> articles = _articleDbContext.Articles
            .Include(art => art.Journal)
            .Include(art => art.AuthorLinks)
            .ThenInclude(authLk => authLk.Scientist);
        return articles.Where(article => article.Category == categoryChoice).ToList();
    }

    public IEnumerable<ScientificArticle> ReadArticlesByScientist(int scientistId)
    {
        var articles = _articleDbContext.Articles
            .Where(article => article.AuthorLinks.Any(authLk => authLk.Scientist.Id == scientistId));

        return articles.ToList();
    }

    public IEnumerable<ScientificArticle> ReadArticlesNotByScientist(int scientistId)
    {
        var articles = _articleDbContext.Articles
            .Where(article => article.AuthorLinks.All(authLk => authLk.Scientist.Id != scientistId));
        return articles.ToList();
    }

    public ScientificArticle ReadArticleById(int id)
    {
        return _articleDbContext.Articles.Find(id);
    }
    
    public ScientificArticle ReadArticleByIdWithDataOwner(int id)
    {
        return _articleDbContext.Articles
            .Include(art => art.DataOwner)
            .SingleOrDefault(art => art.Id == id);
    }
    
    public ScientificArticle ReadArticleByIdWithAuthorsAndJournal(int id)
    {
        var article = _articleDbContext.Articles.Where(art => art.Id == id)
            .Include(art => art.Journal)
            .Include(art => art.AuthorLinks)
            .ThenInclude(authLk => authLk.Scientist)
            .SingleOrDefault();
        return article;
    }

    public ScientificArticle ReadArticleByIdWithAuthorsAndJournalAndDataOwner(int id)
    {
        var article = _articleDbContext.Articles.Where(art => art.Id == id)
            .Include(art => art.Journal)
            .Include(art => art.AuthorLinks)
            .ThenInclude(authLk => authLk.Scientist)
            .Include(art => art.DataOwner)
            .SingleOrDefault();
        return article;
    }

    public void CreateArticle(ScientificArticle articleToInsert)
    {
        _articleDbContext.Articles.Add(articleToInsert);
        _articleDbContext.SaveChanges();
    }
    
    public void UpdateArticle(ScientificArticle scientificArticle)
    {
        _articleDbContext.Articles.Update(scientificArticle);
        _articleDbContext.SaveChanges();
    }

    
    public IEnumerable<Scientist> ReadAllScientists()
    {
        return _articleDbContext.Scientists.ToList();
    }

    public IEnumerable<Scientist> ReadAllScientistsWithArticles()
    {
        IQueryable<Scientist> scientists = _articleDbContext.Scientists
                                                              .Include(s => s.ArticleLinks)
                                                              .ThenInclude(al => al.Article);
        return scientists.ToList();
    }

    public IEnumerable<Scientist> ReadScientistsByNameAndDateOfBirthWithArticles(string nameString, DateOnly? dateOfBirth)
    {
        IQueryable<Scientist> query = _articleDbContext.Scientists;

        if (dateOfBirth != null && dateOfBirth < DateOnly.FromDateTime(DateTime.Now))
        {
            query = query.Where(scientist => scientist.DateOfBirth == dateOfBirth);
        }
        
        string[] scientistNameParts = nameString.Split(" ");
        if (!String.IsNullOrEmpty(scientistNameParts[0]))
        {
            foreach (var namePart in scientistNameParts)
            {
                query = query.Where(s =>s.Name.ToLower().Contains(namePart.ToLower()));
            }
        }

        return query.Include(s => s.ArticleLinks).ThenInclude(al => al.Article).ToList();
    }

    public Scientist ReadScientistById(int id)
    {
        return _articleDbContext.Scientists.Find(id);
    }

    public void CreateScientist(Scientist scientistToInsert)
    {
        _articleDbContext.Scientists.Add(scientistToInsert);
        _articleDbContext.SaveChanges();
    }
    

    public IEnumerable<ArticleScientistLink> ReadArticleScientistLinksByArticleId(int articleId)
    {
        return _articleDbContext.ArticleScientistLinks.Where(asLk => asLk.Article.Id == articleId).ToList();
    }

    public ArticleScientistLink ReadArticleScientistLinkByArticleIdAndScientistId(int articleId, int scientistId)
    {
        return _articleDbContext.ArticleScientistLinks.Find(articleId, scientistId);
    }

    public void CreateArticleScientistLink(ArticleScientistLink articleScientistLink)
    {
        _articleDbContext.ArticleScientistLinks.Add(articleScientistLink);
        _articleDbContext.SaveChanges();
    }

    public void DeleteArticleScientistLink(ArticleScientistLink articleScientistLink)
    {
        _articleDbContext.ArticleScientistLinks.Remove(articleScientistLink);
        _articleDbContext.SaveChanges();
    }

    public IEnumerable<ScienceJournal> ReadAllJournals()
    {
        return _articleDbContext.Journals.ToList();
    }

    public ScienceJournal ReadJournalByIdWithArticles(int id)
    {
        var journal = _articleDbContext.Journals.Where(j => j.Id == id)
            .Include(j => j.Articles)
            .SingleOrDefault();
        return journal;
    }

    public void CreateJournal(ScienceJournal journalToInsert)
    {
        _articleDbContext.Journals.Add(journalToInsert);
        _articleDbContext.SaveChanges();
    }

    public IdentityUser ReadUserByUserName(string userName)
    {
        return _articleDbContext.Users.SingleOrDefault(u => u.UserName == userName);
    }
}