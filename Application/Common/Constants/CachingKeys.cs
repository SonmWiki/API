namespace Application.Common.Constants;

public static class CachingKeys
{
    public static class Articles
    {
        public static string ArticleById(string id) => $"article-by-id-{id}";
    }
    
    public static class Categories
    {
        public const string CategoriesAll = "categories";
        public const string CategoriesTree = "categories-tree";
        public static string CategoryArticlesById(string id) => $"category-articles-by-id-{id}";
    }
    
    public static class Navigation
    {
        public const string NavigationsTree = "navigations-tree";
    }
}