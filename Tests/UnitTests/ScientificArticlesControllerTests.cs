using System.Security.Claims;
using ArticleManagement.BL;
using ArticleManagement.BL.Domain;
using ArticleManagement.UI.Web.Controllers.Api;
using ArticleManagement.UI.Web.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Tests.UnitTests;

public class ScientificArticlesControllerTests
{
    private readonly Mock<IManager> _manager;
    private readonly ScientificArticlesController _controller;
    
    public ScientificArticlesControllerTests()
    {
        _manager = new Mock<IManager>();
        _controller = new ScientificArticlesController(_manager.Object);
    }

    [Fact]
    public void GetArticlesByScientist_GivenInvalidScientistId_ReturnsNotFound()
    {
        // Arrange
        int scientistId = 1;
        _manager.Setup(mgr => mgr.GetScientistById(scientistId)).Returns((Scientist)null);
        
        // Act
        var result = _controller.GetArticlesByScientist(scientistId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void GetArticlesByScientist_GivenValidScientistId_ArticlesNotFound_ReturnsNoContent()
    {
        // Arrange
        int scientistId = 1;
        _manager.Setup(m => m.GetScientistById(scientistId)).Returns(new Scientist("DummyScientist", "fac", "uni"));
        _manager.Setup(m => m.GetArticlesByScientist(scientistId)).Returns(new List<ScientificArticle>());

        // Act
        var result = _controller.GetArticlesByScientist(scientistId);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }
    
    [Fact]
    public void GetArticlesByScientist_GivenValidScientistId_ArticlesFound_ReturnsOkWithArticles()
    {
        // Arrange
        int scientistId = 1;
        var articles = new List<ScientificArticle> { new ScientificArticle("TestArticle1"), new ScientificArticle("TestArticle2")};
        _manager.Setup(mgr => mgr.GetScientistById(scientistId)).Returns(new Scientist("DummyScientist", "fac", "uni"));
        _manager.Setup(mgr => mgr.GetArticlesByScientist(scientistId)).Returns(articles);

        // Act
        var result = _controller.GetArticlesByScientist(scientistId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(articles, okResult.Value);
    }
    
    
     [Fact]
    public void PutArticle_GivenInvalidArticleId_ReturnsNotFound()
    {
        // Arrange
        var updateDto = new UpdateScientificArticleDto { Id = 1, Category = ArticleCategory.Astrophysics };
        _manager.Setup(mgr => mgr.GetArticleByIdWithDataOwner(updateDto.Id)).Returns((ScientificArticle)null);

        // Act
        var result = _controller.PutArticle(updateDto);

        // Assert
        Assert.IsType<NotFoundResult>(result);
        _manager.Verify(mgr => mgr.ChangeArticle(It.IsAny<ScientificArticle>(), It.IsAny<ArticleCategory>()), Times.Never);
    }

    [Fact]
    public void PutArticle_AsUnauthorizedUser_ReturnsForbid()
    {
        // Arrange
        var updateDto = new UpdateScientificArticleDto { Id = 1, Category = ArticleCategory.Chemistry };
        var existingArticle = new ScientificArticle("TestArticle") { Id = 1, DataOwner = new IdentityUser("data.owner@kdg.be") };
        _manager.Setup(mgr => mgr.GetArticleByIdWithDataOwner(updateDto.Id)).Returns(existingArticle);

        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Name, "not.data.owner@kdg.com")
        }, "mockAuthType"));

        _controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = user }
        };

        // Act
        var result = _controller.PutArticle(updateDto);

        // Assert
        Assert.IsType<ForbidResult>(result);
        _manager.Verify(mgr => mgr.ChangeArticle(It.IsAny<ScientificArticle>(), It.IsAny<ArticleCategory>()), Times.Never);
    }

    [Fact]
    public void PutArticle_AsDataOwner_ReturnsOkWithUpdatedArticle()
    {
        // Arrange
        var updateDto = new UpdateScientificArticleDto { Id = 1, Category = ArticleCategory.Astrophysics };
        var existingArticle = new ScientificArticle("TestArticle")
        {
            Id = 1, Category = ArticleCategory.Electromagnetism, DataOwner = new IdentityUser("data.owner@kdg.com")
        };
        var updatedArticle = existingArticle;
        updatedArticle.Category = updateDto.Category;
        _manager.Setup(mgr => mgr.GetArticleByIdWithDataOwner(updateDto.Id))
            .Returns(existingArticle)
            .Verifiable(Times.Once);
        _manager.Setup(mgr => mgr.ChangeArticle(existingArticle, updateDto.Category))
            .Returns(updatedArticle)
            .Verifiable(Times.Once);

        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Name, "data.owner@kdg.com")
        }, "mockAuthType"));

        _controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = user }
        };

        // Act
        var result = _controller.PutArticle(updateDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(updatedArticle, okResult.Value);
        _manager.VerifyAll();
    }

    [Fact]
    public void PutArticle_AsAdmin_ReturnsOkWithUpdatedArticle()
    {
        // Arrange
        var updateDto = new UpdateScientificArticleDto { Id = 1, Category = ArticleCategory.Astrophysics };
        var existingArticle = new ScientificArticle("TestArticle")
        {
            Id = 1, Category = ArticleCategory.Electromagnetism, DataOwner = new IdentityUser("data.owner@kdg.com")
        };
        var updatedArticle = existingArticle;
        updatedArticle.Category = updateDto.Category;
        _manager.Setup(mgr => mgr.GetArticleByIdWithDataOwner(updateDto.Id))
            .Returns(existingArticle)
            .Verifiable(Times.Once);
        _manager.Setup(mgr => mgr.ChangeArticle(existingArticle, updateDto.Category))
            .Returns(updatedArticle)
            .Verifiable(Times.Once);

        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Name, "admin.user@kdg.com"),
            new Claim(ClaimTypes.Role, "Admin")
        }, "mockAuthType"));

        _controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = user }
        };

        // Act
        var result = _controller.PutArticle(updateDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(updatedArticle, okResult.Value);
        _manager.VerifyAll();
    }
}