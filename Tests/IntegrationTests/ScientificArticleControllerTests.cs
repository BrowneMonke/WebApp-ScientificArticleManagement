using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Tests.IntegrationTests;

public class ScientificArticleControllerTests : IClassFixture<ExtendedWebApplicationFactoryWithMockAuth<Program>>
{
    private readonly ExtendedWebApplicationFactoryWithMockAuth<Program> _factory;
    
    public ScientificArticleControllerTests(ExtendedWebApplicationFactoryWithMockAuth<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public void Add_AsAuthenticatedUser_ReturnsAddArticleFormPage()
    {
        // Arrange
        var httpClient = _factory
            .SetAuthenticatedUser(
                new Claim(ClaimTypes.Name, "default.user@kdg.be"))
            .CreateClient();

        string url = $"/ScientificArticle/Add";
        
        // Act
        var response = httpClient.GetAsync(url).GetAwaiter().GetResult();
        string responseBodyAsString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("text/html", response.Content.Headers.ContentType?.MediaType); // response contains html?
        Assert.Contains("Add article", responseBodyAsString);
        
        // clear authenticated user
        _factory.SetAuthenticatedUser();
    }
    
    [Fact]
    public void Add_AsUnauthenticatedUser_ReturnsRedirectToLoginPage()
    {
        // Arrange
        var httpClient = _factory.CreateClient(new WebApplicationFactoryClientOptions()
        {
            AllowAutoRedirect = false
        });

        string url = $"/ScientificArticle/Add";
        
        // Act
        var response = httpClient.GetAsync(url).GetAwaiter().GetResult();
        
        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
    
}