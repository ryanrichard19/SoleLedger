using System.Security.Claims;

namespace SoleLedgerApi;

// A scoped service that exposes the current user information
public class CurrentUser
{
    public SoleLedgerUser? User { get; set; }
    public ClaimsPrincipal Principal { get; set; } = default!;

    public string Id => Principal.FindFirstValue(ClaimTypes.NameIdentifier)!;
    public bool IsAdmin => Principal.IsInRole("admin");
}