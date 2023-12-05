using Microsoft.AspNetCore.Routing;

namespace Application.Features.Categories.Extensions;

public static class EndpointRouteBuilderExt
{
    public static IEndpointRouteBuilder MapCategories(this IEndpointRouteBuilder app)
    {
        GetCategories.Map(app);
        CreateCategory.Map(app);
        UpdateCategory.Map(app);
        DeleteCategory.Map(app);
        GetCategoryArticles.Map(app);
        
        return app;
    }
}