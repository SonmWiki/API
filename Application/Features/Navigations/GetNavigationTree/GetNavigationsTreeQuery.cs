using Application.Common.Constants;
using Application.Common.Messaging;
using ErrorOr;

namespace Application.Features.Navigations.GetNavigationTree;

public record GetNavigationsTreeQuery : ICachedQuery<ErrorOr<GetNavigationsTreeResponse>>
{
    public string Key => CachingKeys.Navigation.NavigationsTree;
    public TimeSpan? Expiration => TimeSpan.FromHours(12);
    public bool IgnoreCaching => false;
}