using System.Reflection;
using Application.Features.Articles.CreateArticle;
using Application.Features.Articles.DeleteArticle;
using Application.Features.Articles.EditArticle;
using Application.Features.Articles.GetArticle;
using Application.Features.Articles.GetPendingRevisions;
using Application.Features.Articles.GetPendingRevisionsCount;
using Application.Features.Articles.GetRevisionHistory;
using Application.Features.Articles.GetRevisionReviewHistory;
using Application.Features.Articles.ReviewRevision;
using Application.Features.Articles.SearchArticles;
using Application.Features.Articles.SetRedirect;
using Application.Features.Authors.CreateAuthor;
using Application.Features.Authors.EditAuthor;
using Application.Features.Categories.CreateCategory;
using Application.Features.Categories.DeleteCategory;
using Application.Features.Categories.GetCategories;
using Application.Features.Categories.GetCategoriesTree;
using Application.Features.Categories.GetCategoryArticles;
using Application.Features.Navigations.GetNavigationTree;
using Application.Features.Navigations.UpdateNavigationsTree;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Slugify;

namespace Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        //TODO: Reimplement without MediatR
        // services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        // services.AddTransient(typeof(IPipelineBehavior<,>), typeof(QueryCachingBehavior<,>));
        services.AddTransient<ISlugHelper, SlugHelperForNonAsciiLanguages>();

        services.AddScoped<ICreateArticleCommandHandler, CreateArticleCommandHandler>();
        services.AddScoped<IDeleteArticleCommandHandler, DeleteArticleCommandHandler>();
        services.AddScoped<IEditArticleCommandHandler, EditArticleCommandHandler>();
        services.AddScoped<IGetArticleQueryHandler, GetArticleQueryHandler>();
        services.AddScoped<IGetPendingRevisionsQueryHandler, GetPendingRevisionsQueryHandler>();
        services.AddScoped<IGetPendingRevisionsCountQueryHandler, GetPendingRevisionsCountQueryHandler>();
        services.AddScoped<IGetRevisionHistoryQueryHandler, GetRevisionHistoryQueryHandler>();
        services.AddScoped<IGetRevisionReviewHistoryQueryHandler, GetRevisionReviewHistoryQueryHandler>();
        services.AddScoped<IReviewRevisionCommandHandler, ReviewRevisionCommandHandler>();
        services.AddScoped<ISearchArticlesQueryHandler, SearchArticlesQueryHandler>();
        services.AddScoped<ISetRedirectCommandHandler, SetRedirectCommandHandler>();

        services.AddScoped<ICreateAuthorCommandHandler, CreateAuthorCommandHandler>();
        services.AddScoped<IEditAuthorCommandHandler, EditAuthorCommandHandler>();

        services.AddScoped<ICreateCategoryCommandHandler, CreateCategoryCommandHandler>();
        services.AddScoped<IDeleteCategoryCommandHandler, DeleteCategoryCommandHandler>();
        services.AddScoped<IGetCategoriesQueryHandler, GetCategoriesQueryHandler>();
        services.AddScoped<IGetCategoriesTreeQueryHandler, GetCategoriesTreeQueryHandler>();
        services.AddScoped<IGetCategoryArticlesQueryHandler, GetCategoryArticlesQueryHandler>();

        services.AddScoped<IGetNavigationsTreeQueryHandler, GetNavigationsTreeQueryHandler>();
        services.AddScoped<IUpdateNavigationsTreeCommandHandler, UpdateNavigationsTreeCommandHandler>();

        return services;
    }
}