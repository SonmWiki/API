using Application.Common.Constants;
using Application.Common.Messaging;

namespace Application.Features.Navigations.GetNavigationTree;

public record GetNavigationsTreeQuery : ICachedQuery<GetNavigationsTreeResponse>
{
    public string Key => CachingKeys.Navigation.NavigationsTree;
    public TimeSpan? Expiration => TimeSpan.FromHours(12);
    public bool IgnoreCaching => false;
}