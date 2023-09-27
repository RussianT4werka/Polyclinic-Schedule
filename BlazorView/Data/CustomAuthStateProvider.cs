using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    ClaimsIdentity currentUser = GetAnon();

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(currentUser);
        return Task.FromResult(new AuthenticationState(claimsPrincipal));
    }

    private static ClaimsIdentity GetAnon()
    {
        return new ClaimsIdentity(
                 new[]{  new Claim(ClaimTypes.Name, "Anon"),
                });
    }

    public void AuthUser(string session, int id)
    {
        currentUser = new ClaimsIdentity(
                 new[]{
                     new Claim(ClaimTypes.Sid, id.ToString()),
                     new Claim(ClaimTypes.Name, session),
                }, "Auth");
        var task = GetAuthenticationStateAsync();
        NotifyAuthenticationStateChanged(task);
    }

    public void Logout()
    {
        currentUser = GetAnon();
    }
}