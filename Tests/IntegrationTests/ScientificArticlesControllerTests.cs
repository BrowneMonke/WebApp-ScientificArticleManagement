using System.Net;
using System.Security.Claims;
using System.Text;
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
        // Arrange
        var httpClient = _factory.CreateClient(); // no authentication

        const int articleId = 1;
        const ArticleCategory newArticleCategory = (ArticleCategory) 5;

        UpdateScientificArticleDto updateScientificArticleDto = new UpdateScientificArticleDto()
        {
            Id = articleId,
            Category = newArticleCategory
        };        
        var updatedScientificArticleJson = new StringContent(JsonConvert.SerializeObject(updateScientificArticleDto), Encoding.UTF8, "application/json");

        string url = "/api/ScientificArticles/";
        
        // Act
        var response = httpClient.PutAsync(url, updatedScientificArticleJson).GetAwaiter().GetResult();
        
        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
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
        
        // clear authenticated user
        _factory.SetAuthenticatedUser();
    }
    
    
    [Fact]
    public void PutArticle_AuthenticatedButUnauthorizedUser_GivenValidData_ReturnsForbidden()
    {
        // Arrange
        var httpClient = _factory
            .SetAuthenticatedUser(
                new Claim(ClaimTypes.Name, "authenticated.user@kdg.be")
            )
            .CreateClient( new WebApplicationFactoryClientOptions()
            {
                AllowAutoRedirect = false
            });
        
        const int articleId = 1;
        const ArticleCategory newArticleCategory = (ArticleCategory) 5;

        UpdateScientificArticleDto updateScientificArticleDto = new UpdateScientificArticleDto()
        {
            Id = articleId,
            Category = newArticleCategory
        };
        var updatedScientificArticleJson = new StringContent(JsonConvert.SerializeObject(updateScientificArticleDto), Encoding.UTF8, "application/json");

        string url = "/api/ScientificArticles/";
        
        // Act
        var response = httpClient.PutAsync(url, updatedScientificArticleJson).GetAwaiter().GetResult();
        
        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        
        // clear authenticated user
        _factory.SetAuthenticatedUser();
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

        const int articleId = 1;
        const ArticleCategory newArticleCategory = (ArticleCategory) 5;

        UpdateScientificArticleDto updateScientificArticleDto = new UpdateScientificArticleDto()
        {
            Id = articleId,
            Category = newArticleCategory
        };
        var updatedScientificArticleJson = new StringContent(JsonConvert.SerializeObject(updateScientificArticleDto), Encoding.UTF8, "application/json");

        string url = "/api/ScientificArticles/";
        
        // Act
        var response = httpClient.PutAsync(url, updatedScientificArticleJson).GetAwaiter().GetResult();
        string responseBodyAsString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        using var scope = _factory.Services.CreateScope();
        var ctx = scope.ServiceProvider.GetService<ArticleDbContext>();
        var selectedArticle = ctx.Articles.Find(articleId);
        var updatedArticle = JsonConvert.DeserializeObject<ScientificArticle>(responseBodyAsString);

        Assert.NotNull(selectedArticle);
        Assert.Equal(selectedArticle.Id, updatedArticle.Id);
        Assert.Equal(selectedArticle.Title, updatedArticle.Title);
        Assert.Equal(selectedArticle.NumberOfPages, updatedArticle.NumberOfPages);
        Assert.Equal(selectedArticle.DateOfPublication, updatedArticle.DateOfPublication);
        Assert.Equal(newArticleCategory, updatedArticle.Category);
        
        // clear authenticated user
        _factory.SetAuthenticatedUser();
    }

    
}