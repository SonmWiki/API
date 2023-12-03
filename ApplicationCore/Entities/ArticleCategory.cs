namespace Application.Entities;

public class ArticleCategory
{
    public required string ArticleId { get; set; }
    public required Article Article { get; set; }
    public required string CategoryId { get; set; }
    public required Category Category { get; set; }
}