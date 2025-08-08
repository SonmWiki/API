using System.Reflection;
using Application.Common.Messaging;
using Application.Authorization;
using Application.Authorization.Abstractions;
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
using Application.Features.Categories.CreateCategory;
using Application.Features.Categories.DeleteCategory;
using Application.Features.Categories.GetCategories;
using Application.Features.Categories.GetCategoriesTree;
using Application.Features.Categories.GetCategoryArticles;
using Application.Features.Navigations.GetNavigationTree;
using Application.Features.Navigations.UpdateNavigationsTree;
using Application.Features.Users.GetUser;
using Application.Features.Users.RenameUser;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Slugify;

namespace Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddTransient<ISlugHelper, SlugHelperForNonAsciiLanguages>();

        services.AddScoped<IPermissionService, PermissionService>();

        services.AddScoped<ICommandHandler<CreateArticleCommand, CreateArticleResponse>, CreateArticleCommandHandler>();
        services.AddScoped<ICommandHandler<DeleteArticleCommand, DeleteArticleResponse>, DeleteArticleCommandHandler>();
        services.AddScoped<ICommandHandler<EditArticleCommand, EditArticleResponse>, EditArticleCommandHandler>();
        services.AddScoped<IQueryHandler<GetArticleQuery, GetArticleResponse>, GetArticleQueryHandler>();
        services.AddScoped<IQueryHandler<GetPendingRevisionsQuery, GetPendingRevisionsResponse>, GetPendingRevisionsQueryHandler>();
        services.AddScoped<IQueryHandler<GetPendingRevisionsCountQuery, GetPendingRevisionsCountResponse>, GetPendingRevisionsCountQueryHandler>();
        services.AddScoped<IQueryHandler<GetRevisionHistoryQuery, GetRevisionHistoryResponse>, GetRevisionHistoryQueryHandler>();
        services.AddScoped<IQueryHandler<GetRevisionReviewHistoryQuery, GetRevisionReviewHistoryResponse>, GetRevisionReviewHistoryQueryHandler>();
        services.AddScoped<ICommandHandler<ReviewRevisionCommand, ReviewRevisionResponse>, ReviewRevisionCommandHandler>();
        services.AddScoped<IQueryHandler<SearchArticlesQuery, SearchArticlesResponse>, SearchArticlesQueryHandler>();
        services.AddScoped<ICommandHandler<SetRedirectCommand, SetRedirectResponse>, SetRedirectCommandHandler>();

        services.AddScoped<IRenameUserCommandHandler, RenameUserCommandHandler>();
        services.AddScoped<IGetUserCommandHandler, GetUserCommandHandler>();

        services.AddScoped<ICommandHandler<CreateCategoryCommand, CreateCategoryResponse>, CreateCategoryCommandHandler>();
        services.AddScoped<ICommandHandler<DeleteCategoryCommand, DeleteCategoryResponse>, DeleteCategoryCommandHandler>();
        services.AddScoped<IQueryHandler<GetCategoriesQuery, GetCategoriesResponse>, GetCategoriesQueryHandler>();
        services.AddScoped<IQueryHandler<GetCategoriesTreeQuery, GetCategoriesTreeResponse>, GetCategoriesTreeQueryHandler>();
        services.AddScoped<IQueryHandler<GetCategoryArticlesQuery, GetCategoryArticlesResponse>, GetCategoryArticlesQueryHandler>();

        services.AddScoped<IQueryHandler<GetNavigationsTreeQuery, GetNavigationsTreeResponse>, GetNavigationsTreeQueryHandler>();
        services.AddScoped<ICommandHandler<UpdateNavigationsTreeCommand, UpdateNavigationsTreeResponse>, UpdateNavigationsTreeCommandHandler>();

        return services;
    }
}