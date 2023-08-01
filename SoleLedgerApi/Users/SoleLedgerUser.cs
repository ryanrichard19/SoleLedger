using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace SoleLedgerApi;

// This is our SoleLedgerUser, we can modify this if we need
// to add custom properties to the user
public class SoleLedgerUser : IdentityUser { }

// This is the DTO used to exchange username and password details to 
// the create user and token endpoints
public class UserInfo
{
    [Required]
    public string Username { get; set; } = default!;

    [Required]
    public string Password { get; set; } = default!;
}

public class ExternalUserInfo
{
    [Required]
    public string Username { get; set; } = default!;

    [Required]
    public string ProviderKey { get; set; } = default!;
}
