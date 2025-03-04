using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using rescute.Application.Samaritans;
using rescute.Domain.Aggregates;

namespace rescute.Web.Authentication;

public class CurrentUserService : ICurrentUserService
{
    private readonly ISamaritansService samaritansService;
    private Samaritan? currentUser;

    public CurrentUserService(AuthenticationStateProvider authenticationStateProvider,
        ISamaritansService samaritansService)
    {
        this.samaritansService = samaritansService;
        authenticationStateProvider.AuthenticationStateChanged += OnAuthenticationStateChanged;
    }

    private async void OnAuthenticationStateChanged(Task<AuthenticationState> getAuthState)
    {
        var authState = await getAuthState;
        var userPrincipal = authState.User;
        if (userPrincipal.Identity is { IsAuthenticated: false })
        {
            currentUser = null;
            return;
        }

        var userId = userPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!string.IsNullOrEmpty(userId))
        {
            currentUser = await samaritansService.GetOneSamaritan(); // TODO: get the actual user
            return;
        }

        currentUser = null;
    }

    public Samaritan? GetCurrentUser() => samaritansService.GetOneSamaritan().Result; //currentUser;
}