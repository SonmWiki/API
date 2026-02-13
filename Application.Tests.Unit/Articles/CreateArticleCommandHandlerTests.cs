using Application.Authorization.Abstractions;
using Application.Data;
using Application.Features.Articles.CreateArticle;
using Domain.Entities;
using FluentValidation;
using MockQueryable.Moq;
using Slugify;
using FluentValidation.Results;

namespace Application.Tests.Unit.Articles;

public class CreateArticleCommandHandlerTests
{
    private readonly Mock<IApplicationDbContext> _mockDbContext = new();
    private readonly Mock<ISlugHelper> _mockSlugHelper = new();
    private readonly Mock<IUserContext> _mockUserContext = new();
    private readonly Mock<IValidator<CreateArticleCommand>> _mockValidator = new();
    private readonly CreateArticleCommandHandler _handler;

    public CreateArticleCommandHandlerTests()
    {
        _handler = new CreateArticleCommandHandler(
            _mockDbContext.Object,
            _mockSlugHelper.Object,
            _mockUserContext.Object,
            _mockValidator.Object
        );
    }

    [Fact]
    public async Task Handle_Should_ReturnFailureResult_WhenGeneratedIdIsEmpty()
    {
        //Arrange
        var mockArticlesDbSet = new List<Article>()
            .BuildMockDbSet();

        _mockDbContext.Setup(x => x.Articles).Returns(mockArticlesDbSet.Object);
        _mockSlugHelper.Setup(x => x.GenerateSlug(" ")).Returns("");
        _mockValidator.Setup(v => v.Validate(It.IsAny<CreateArticleCommand>()))
            .Returns(new ValidationResult());

        var command = new CreateArticleCommand(" ", "Lorem ipsum", "", []);

        //Act
        var result = await _handler.HandleAsync(command, default);

        //Assert
        result.IsError.ShouldBeTrue();
        result.FirstError.ShouldBe(Features.Articles.Errors.Article.EmptyId);
    }

    [Fact]
    public async Task Handle_Should_NotCallSaveChangesAsync_WhenGeneratedIdIsEmpty()
    {
        //Arrange
        var mockArticlesDbSet = new List<Article>()
            .BuildMockDbSet();

        _mockDbContext.Setup(x => x.Articles).Returns(mockArticlesDbSet.Object);
        _mockSlugHelper.Setup(x => x.GenerateSlug(" ")).Returns("");
        _mockValidator.Setup(v => v.Validate(It.IsAny<CreateArticleCommand>()))
            .Returns(new ValidationResult());

        var command = new CreateArticleCommand(" ", "Lorem ipsum", "", []);

        //Act
        await _handler.HandleAsync(command, default);

        //Assert
        _mockDbContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailureResult_WhenGeneratedIdIsNotUnique()
    {
        //Arrange
        var mockArticlesDbSet = new List<Article> {new() {Id = "something-cool", Title = "something-cool"}}
            .BuildMockDbSet();

        _mockDbContext.Setup(x => x.Articles).Returns(mockArticlesDbSet.Object);
        _mockSlugHelper.Setup(x => x.GenerateSlug("something cool")).Returns("something-cool");
        _mockValidator.Setup(v => v.Validate(It.IsAny<CreateArticleCommand>()))
            .Returns(new ValidationResult());

        var command = new CreateArticleCommand("something cool", "Lorem ipsum", "", []);

        //Act
        var result = await _handler.HandleAsync(command, default);

        //Assert
        result.IsError.ShouldBeTrue();
        result.FirstError.ShouldBe(Features.Articles.Errors.Article.DuplicateId);
    }

    [Fact]
    public async Task Handle_Should_NotCallSaveChangesAsync_WhenGeneratedIdIsNotUnique()
    {
        //Arrange
        var mockArticlesDbSet = new List<Article> {new() {Id = "something-cool", Title = "something-cool"}}
            .BuildMockDbSet();

        _mockDbContext.Setup(x => x.Articles).Returns(mockArticlesDbSet.Object);
        _mockSlugHelper.Setup(x => x.GenerateSlug("something cool")).Returns("something-cool");
        _mockValidator.Setup(v => v.Validate(It.IsAny<CreateArticleCommand>()))
            .Returns(new ValidationResult());

        var command = new CreateArticleCommand("something cool", "Lorem ipsum", "", []);

        //Act
        var result = await _handler.HandleAsync(command, default);

        //Assert
        _mockDbContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_CallSaveChangesAsync_WhenGeneratedIdIsUnique()
    {
        //Arrange
        var mockArticlesDbSet = new List<Article> {new() {Id = "something-cool", Title = "something-cool"}}
            .BuildMockDbSet();
        var mockCategoriesDbSet = new List<Category>()
            .BuildMockDbSet();
        var mockRevisionsDbSet = new List<Revision>()
            .BuildMockDbSet();

        _mockDbContext.Setup(x => x.Articles).Returns(mockArticlesDbSet.Object);
        _mockDbContext.Setup(x => x.Categories).Returns(mockCategoriesDbSet.Object);
        _mockDbContext.Setup(x => x.Revisions).Returns(mockRevisionsDbSet.Object);
        _mockUserContext.Setup(x => x.UserId).Returns(Guid.Empty);
        _mockSlugHelper.Setup(x => x.GenerateSlug("something extra cool")).Returns("something-extra-cool");
        _mockValidator.Setup(v => v.Validate(It.IsAny<CreateArticleCommand>()))
            .Returns(new ValidationResult());
        var command = new CreateArticleCommand("something extra cool", "Lorem ipsum", "", []);

        //Act
        await _handler.HandleAsync(command, default);

        //Assert
        _mockDbContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccessResultWithArticleId_WhenGeneratedIdIsUnique()
    {
        //Arrange
        var mockArticlesDbSet = new List<Article> {new() {Id = "something-cool", Title = "something-cool"}}
            .BuildMockDbSet();
        var mockCategoriesDbSet = new List<Category>()
            .BuildMockDbSet();
        var mockRevisionsDbSet = new List<Revision>()
            .BuildMockDbSet();

        _mockDbContext.Setup(x => x.Articles).Returns(mockArticlesDbSet.Object);
        _mockDbContext.Setup(x => x.Categories).Returns(mockCategoriesDbSet.Object);
        _mockDbContext.Setup(x => x.Revisions).Returns(mockRevisionsDbSet.Object);
        _mockUserContext.Setup(x => x.UserId).Returns(Guid.Empty);
        _mockSlugHelper.Setup(x => x.GenerateSlug("something extra cool")).Returns("something-extra-cool");
        _mockValidator.Setup(v => v.Validate(It.IsAny<CreateArticleCommand>()))
            .Returns(new ValidationResult());

        var command = new CreateArticleCommand("something extra cool", "Lorem ipsum", "", []);

        //Act
        var result = await _handler.HandleAsync(command, default);

        //Assert
        _mockDbContext.Verify(x => x.Articles.AddAsync(
                It.Is<Article>(e => e.Id == result.Value.Id),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );
        result.IsError.ShouldBeFalse();
        result.Value.ShouldBeOfType<CreateArticleResponse>();
        result.Value.Id.ShouldBe("something-extra-cool");
    }

    [Fact]
    public async Task Handle_Should_CreateRevisionWithContent_WhenGeneratedIdIsUnique()
    {
        //Arrange
        var mockArticlesDbSet = new List<Article> {new() {Id = "something-cool", Title = "something-cool"}}
            .BuildMockDbSet();
        var mockCategoriesDbSet = new List<Category>
            {
                new() {Id = "category1", Name = "Category 1"},
                new() {Id = "category2", Name = "Category 2"}
            }
            .BuildMockDbSet();
        var mockRevisionsDbSet = new List<Revision>()
            .BuildMockDbSet();

        _mockDbContext.Setup(x => x.Articles).Returns(mockArticlesDbSet.Object);
        _mockDbContext.Setup(x => x.Categories).Returns(mockCategoriesDbSet.Object);
        _mockDbContext.Setup(x => x.Revisions).Returns(mockRevisionsDbSet.Object);
        _mockUserContext.Setup(x => x.UserId).Returns(Guid.Empty);
        _mockSlugHelper.Setup(x => x.GenerateSlug("something extra cool")).Returns("something-extra-cool");
        _mockValidator.Setup(v => v.Validate(It.IsAny<CreateArticleCommand>()))
            .Returns(new ValidationResult());
        var expectedCategoryIds = new List<string> {"category1", "category2"};
        var command = new CreateArticleCommand("something extra cool", "Lorem ipsum", "",
            ["category1", "category2", "category3"]);

        //Act
        var result = await _handler.HandleAsync(command, default);

        //Assert
        _mockDbContext.Verify(x => x.Revisions.AddAsync(
                It.Is<Revision>(e =>
                    e.ArticleId == result.Value.Id
                    && e.Content == command.Content
                    && e.AuthorsNote == command.AuthorsNote
                    && e.AuthorId == _mockUserContext.Object.UserId
                    && e.Categories.Select(i => i.Id).OrderBy(i => i).SequenceEqual(expectedCategoryIds)
                ),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );
    }
}