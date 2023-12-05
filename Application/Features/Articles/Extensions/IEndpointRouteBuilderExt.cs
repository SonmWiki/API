using Microsoft.AspNetCore.Routing;

namespace Application.Features.Articles.Extensions;

public static class EndpointRouteBuilderExt
{
    public static IEndpointRouteBuilder MapArticles(this IEndpointRouteBuilder app)
    {
        GetArticle.Map(app);
        CreateArticle.Map(app);
        DeleteArticle.Map(app);
        
        return app;
    }
}