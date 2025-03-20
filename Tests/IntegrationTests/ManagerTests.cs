
using System.ComponentModel.DataAnnotations;
using ArticleManagement.BL;
using ArticleManagement.BL.Domain;
using ArticleManagement.DAL.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.IntegrationTests;

public class ManagerTests : IClassFixture<ExtendedWebApplicationFactoryWithMockAuth<Program>>
{
    private readonly ExtendedWebApplicationFactoryWithMockAuth<Program> _factory;
    
    public ManagerTests(ExtendedWebApplicationFactoryWithMockAuth<Program> factory)
    {
        _factory = factory;
        
    }
    
    [Fact]
    public void AddArticle_GivenValidData_SavesDataToDb_And_ReturnsCreatedArticleObject()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        IManager mgr = scope.ServiceProvider.GetService<IManager>();

        string title = "The Javan Ape";
        List<Scientist> auths = [];
        DateOnly dop = DateOnly.FromDateTime(DateTime.Today);
        int nop = 9;
        ArticleCategory cat = ArticleCategory.Astrophysics;
        string userName = "bob@kdg.be";

        // Act
        ScientificArticle createdArticle = mgr.AddArticle(title, auths, dop, nop, cat, userName);


        // Assert
        Assert.NotNull(createdArticle);
        Assert.False(createdArticle.Id == default);
        Assert.Equal(title, createdArticle.Title);
        Assert.Equal(dop, createdArticle.DateOfPublication);
        Assert.Equal(nop, createdArticle.NumberOfPages);
        Assert.Equal(cat, createdArticle.Category);
        Assert.Equal(userName, createdArticle.DataOwner.UserName);
        
        // is data inserted in db?
        ArticleDbContext ctx = scope.ServiceProvider.GetService<ArticleDbContext>();
        Assert.Equal(createdArticle, ctx.Articles.Find(createdArticle.Id));
    }
    
    [Fact]
    public void AddArticle_GivenInvalidData_ThrowsValidationException()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var mgr = scope.ServiceProvider.GetService<IManager>();

        string title = "Negative pages hehe"; // empty title not allowed
        List<Scientist> auths = [];
        DateOnly dop = DateOnly.FromDateTime(DateTime.Today);
        int nop = -2; // negative number of pages not allowed
        ArticleCategory cat = ArticleCategory.Astrophysics;
        string userName = "bob@kdg.be";

        // Act
        // Book createdBook = mgr.AddBook(isbn, title, format);
        var wrappedActCall = new Func<ScientificArticle>(() => mgr.AddArticle(title, auths, dop, nop, cat, userName));

        // Assert
        Assert.Throws<ValidationException>(wrappedActCall);
    }  
    
    [Fact]
    public void ChangeArticle_GivenValidData_UpdatesDataInDb_And_ReturnsUpdatedArticleObject()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        IManager mgr = scope.ServiceProvider.GetService<IManager>();
        ArticleDbContext ctx = scope.ServiceProvider.GetService<ArticleDbContext>();

        var selectedArticle = ctx.Articles.Include(art => art.DataOwner).FirstOrDefault(); // Selected article 0
        var newArticleCategory = (ArticleCategory) 3;
        

        // Act
        ScientificArticle updatedArticle = mgr.ChangeArticle(selectedArticle, newArticleCategory);


        // Assert
        Assert.NotNull(selectedArticle);
        Assert.NotNull(updatedArticle);
        Assert.True(updatedArticle.Id == selectedArticle.Id);
        Assert.Equal(updatedArticle.Title, selectedArticle.Title);
        Assert.Equal(updatedArticle.DateOfPublication, selectedArticle.DateOfPublication);
        Assert.Equal(updatedArticle.NumberOfPages, selectedArticle.NumberOfPages);
        Assert.Equal(updatedArticle.Category, newArticleCategory);
        
        
        // is data updated in db?
        Assert.Equal(updatedArticle, ctx.Articles.Find(updatedArticle.Id));
    }
    
    [Fact]
    public void ChangeArticle_GivenInvalidData_ThrowsValidationException()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var mgr = scope.ServiceProvider.GetService<IManager>();
        ArticleDbContext ctx = scope.ServiceProvider.GetService<ArticleDbContext>();

        var selectedArticle = ctx.Articles.FirstOrDefault();
        var newArticleCategory = (ArticleCategory) 9; // Invalid category number
        
        
        // Act
        // Book createdBook = mgr.AddBook(isbn, title, format);
        var wrappedActCall = new Func<ScientificArticle>(() => mgr.ChangeArticle(selectedArticle, newArticleCategory));

        // Assert
        Assert.Throws<ValidationException>(wrappedActCall);
    }
    
    
}