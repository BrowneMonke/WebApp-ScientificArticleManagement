using System.ComponentModel.DataAnnotations;
using ArticleManagement.BL;
using ArticleManagement.BL.Domain;
using ArticleManagement.DAL;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Tests.UnitTests;

public class ManagerTests
{
    private readonly IManager _manager;
    private readonly Mock<IRepository> _repository;

    public ManagerTests()
    {
        _repository = new Mock<IRepository>();
        _manager = new Manager(_repository.Object);
    }

    [Fact]
    public void AddArticle_GivenValidData_SavesDataToDb_And_ReturnsCreatedArticle()
    {
        // Arrange
        string title = "Testing title";
        List<Scientist> authors = [];
        DateOnly dateOfPublication = DateOnly.FromDateTime(DateTime.Today);
        int numberOfPages = 9;
        ArticleCategory category = ArticleCategory.Astrophysics;
        string userName = "data.owner@kdg.be";

        _repository.Setup(repo => repo.ReadUserByUserName(userName))
            .Returns(new IdentityUser());
        _repository.Setup(repo => repo.CreateArticle(It.IsAny<ScientificArticle>()))
            .Verifiable(Times.Once);


        // Act
        ScientificArticle createdArticle = _manager.AddArticle(title, authors, dateOfPublication, numberOfPages, category, userName);

        // Assert
        Assert.NotNull(createdArticle);
        Assert.Equal(title, createdArticle.Title);
        Assert.Equal(dateOfPublication, createdArticle.DateOfPublication);
        Assert.Equal(numberOfPages, createdArticle.NumberOfPages);
        Assert.Equal(category, createdArticle.Category);
        _repository.VerifyAll();
    }

    [Fact]
    public void AddArticle_GivenInvalidData_ThrowsValidationException()
    {
        // Arrange
        string title = "Nonexistent ArticleCategory";
        List<Scientist> authors = [];
        DateOnly dateOfPublication = DateOnly.FromDateTime(DateTime.Today);
        int numberOfPages = 2;
        ArticleCategory category = (ArticleCategory)11; // category doesn't exist
        string userName = "alice@kdg.be";

        // Act
        var wrappedActCall = new Func<ScientificArticle>(() =>
            _manager.AddArticle(title, authors, dateOfPublication, numberOfPages, category, userName));

        // Assert
        Assert.Throws<ValidationException>(wrappedActCall);
        _repository.Verify(repo => repo.CreateArticle(It.IsAny<ScientificArticle>()), Times.Never());
    }

    [Fact]
    public void ChangeArticle_GivenValidData_UpdatesDataInDb_And_ReturnsUpdatedArticleObject()
    {
        // Arrange
        var existingArticle = new ScientificArticle("test article")
        {
            AuthorLinks = [], Category = ArticleCategory.Chemistry, NumberOfPages = 3,
            DataOwner = new IdentityUser(), DateOfPublication = new DateOnly(1996, 2, 2)
        };

        var newCategory = ArticleCategory.Biology;

        // Act
        var updatedArticle = _manager.ChangeArticle(existingArticle, newCategory);

        // Assert
        Assert.Equal(existingArticle.Title, updatedArticle.Title);
        Assert.Equal(existingArticle.NumberOfPages, updatedArticle.NumberOfPages);
        Assert.Equal(existingArticle.DataOwner, updatedArticle.DataOwner);
        Assert.Equal(newCategory, updatedArticle.Category);
        _repository.Verify(repo => repo.UpdateArticle(existingArticle), Times.Once);
    }


    [Fact]
    public void ChangeArticle_GivenInvalidData_ThrowsValidationException()
    {
        // Arrange
        var existingArticle = new ScientificArticle("test article")
        {
            AuthorLinks = [], Category = ArticleCategory.Chemistry, NumberOfPages = 3,
            DataOwner = new IdentityUser(), DateOfPublication = new DateOnly(1996, 2, 2)
        };

        var newCategory = (ArticleCategory)11;

        // Act
        var wrappedActCall = new Func<ScientificArticle>(() => _manager.ChangeArticle(existingArticle, newCategory));

        // Assert
        Assert.Throws<ValidationException>(wrappedActCall);
        _repository.Verify(repo => repo.UpdateArticle(It.IsAny<ScientificArticle>()), Times.Never);
    }

}