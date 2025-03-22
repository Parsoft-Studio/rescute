using rescute.Domain.Aggregates;

namespace rescute.Web.Authentication;

/// <summary>
/// Provides access to the currently logged-in user.
/// </summary>
public interface ICurrentUserService
{
    Samaritan? GetCurrentUser();
}