/* Shared classes can be referenced by both the Client and Server */
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class ClientDto
{
    public int Id { get; set; }
    [Required]
    public string ClientName { get; set; } = default!;

    public string? ClientAddress { get; set; }
 
    public string? ContactPerson { get; set; }

    [Required]
    public string ContactEmail { get; set; }

    public string? ContactPhone { get; set; }
}

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

public record AuthToken([property:JsonPropertyName("access_token")]string Token);