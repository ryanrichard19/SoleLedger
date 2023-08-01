using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

public class Client
{
    public int Id { get; set; }

    [Required]
    public string SoleLedgerUserId { get; set; } = default!;

    [Required]
    public string ClientName { get; set; } = default!;

    public string? ClientAddress { get; set; }

    public string? ContactPerson { get; set; }

    [Required]
    public string ContactEmail { get; set; } = default!;

    public string? ContactPhone { get; set; }
}

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

public static class ClientMappingExtensions
{
    public static ClientDto ToDto(this Client client)
    {
        return new()
        {
            Id = client.Id,
            ClientName = client.ClientName,
            ClientAddress = client.ClientAddress,
            ContactPerson = client.ContactPerson,
            ContactEmail = client.ContactEmail,
            ContactPhone = client.ContactPhone
        };
    }
}