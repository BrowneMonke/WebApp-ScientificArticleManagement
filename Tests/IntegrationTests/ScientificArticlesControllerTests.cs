using System.Net;
using System.Security.Claims;
using System.Text;
using ArticleManagement.BL;
using ArticleManagement.BL.Domain;
using ArticleManagement.DAL.EF;
using ArticleManagement.UI.Web.Models.Dto;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Tests.IntegrationTests;

public class ScientificArticlesControllerTests : IClassFixture<ExtendedWebApplicationFactoryWithMockAuth<Program>>
{
    private readonly ExtendedWebApplicationFactoryWithMockAuth<Program> _factory;

    public ScientificArticlesControllerTests(ExtendedWebApplicationFactoryWithMockAuth<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public void PutArticle_AsUnauthenticatedUser_ReturnsUnauthorizedStatusCode()
    {
        var httpClient = _factory.CreateClient();
        
    }
    
        
    [Fact]
    public void PutArticle_AsAuthenticatedUser_GivenInvalidData_ReturnsNotFoundStatusCode()
    {
        // Arrange
        var httpClient = _factory
            .SetAuthenticatedUser(
                new Claim(ClaimTypes.Name, "authenticated.user@kdg.be")
            )
            .CreateClient();
        
        const ArticleCategory newArticleCategory = (ArticleCategory) 5;
        UpdateScientificArticleDto updateScientificArticleDto = new UpdateScientificArticleDto() // no existing article selected
        {
            Category = newArticleCategory
        };
        var updatedScientificArticleJson = new StringContent(JsonConvert.SerializeObject(updateScientificArticleDto), Encoding.UTF8, "application/json");

        string url = "/api/ScientificArticles/";
        
        // Act
        var response = httpClient.PutAsync(url, updatedScientificArticleJson).GetAwaiter().GetResult();
        
        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    
    [Fact]
    public void PutArticle_AsAuthorizedUser_GivenValidData_ReturnsUpdatedArticle()
    {
        // Arrange
        var httpClient = _factory
            .SetAuthenticatedUser(
                new Claim(ClaimTypes.Name, "new.admin@kdg.be"),
                new Claim(ClaimTypes.Role, "Admin")
            )
            .CreateClient();
        
        using var scope = _factory.Services.CreateScope();
        var mgr = scope.ServiceProvider.GetService<IManager>();
        var ctx = scope.ServiceProvider.GetService<ArticleDbContext>();

        const ArticleCategory newArticleCategory = (ArticleCategory) 5;
        var selectedArticle = ctx.Articles.FirstOrDefault();
        if (selectedArticle == null) return; // ASK TEACHER!!

        UpdateScientificArticleDto updateScientificArticleDto = new UpdateScientificArticleDto()
        {
            Id = selectedArticle.Id,
            Category = newArticleCategory
        };
        var updatedScientificArticleJson = new StringContent(JsonConvert.SerializeObject(updateScientificArticleDto), Encoding.UTF8, "application/json");

        string url = "/api/ScientificArticles/";
        
        // Act
        var response = httpClient.PutAsync(url, updatedScientificArticleJson).GetAwaiter().GetResult();
        string responseBodyAsString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var updatedArticle = JsonConvert.DeserializeObject<ScientificArticle>(responseBodyAsString);
        Assert.Equal(selectedArticle.Id, updatedArticle.Id);
        Assert.Equal(selectedArticle.Title, updatedArticle.Title);
        Assert.Equal(selectedArticle.NumberOfPages, updatedArticle.NumberOfPages);
        Assert.Equal(selectedArticle.DateOfPublication, updatedArticle.DateOfPublication);
        Assert.Equal(newArticleCategory, updatedArticle.Category);
    }

    
}