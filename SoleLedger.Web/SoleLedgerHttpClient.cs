using System.Net;

namespace SoleLedger.Web.Client;

public class SoleLedgerClient(HttpClient httpclient)
{
    public async Task<ClientDto?> AddClientAsync(ClientDto client)
    {
        if (string.IsNullOrEmpty(client.ClientName) || string.IsNullOrEmpty(client.ContactEmail))
        {
            return null;
        }

        ClientDto? createdClient = null;

        var response = await httpclient.PostAsJsonAsync("clients", new ClientDto 
            { ClientName = client.ClientName
            , ClientAddress = client.ClientAddress
            , ContactPerson = client.ContactPerson
            , ContactEmail = client.ContactEmail
            , ContactPhone = client.ContactPhone
             });

        if (response.IsSuccessStatusCode)
        {
            createdClient = await response.Content.ReadFromJsonAsync<ClientDto>();
        }

        return createdClient;
    }

    public async Task<bool> DeleteClientAsync(int id)
    {
        var response = await httpclient.DeleteAsync($"clients/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<(HttpStatusCode, List<ClientDto>?)> GetClientAsync()
    {
        // This is a hack from hell to avoid having to know if this is running server or client side
        if (httpclient.BaseAddress is null)
        {
            return (HttpStatusCode.OK, new());
        }

        var response = await httpclient.GetAsync("clients");
        var statusCode = response.StatusCode;
        List<ClientDto>? clients = null;

        if (response.IsSuccessStatusCode)
        {
            clients = await response.Content.ReadFromJsonAsync<List<ClientDto>>();
        }

        return (statusCode, clients);
    }

    public async Task<bool> LoginAsync(string? username, string? password)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            return false;
        }

        var response = await httpclient.PostAsJsonAsync("auth/login", new UserInfo { Username = username, Password = password });
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> CreateUserAsync(string? username, string? password)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            return false;
        }

        var response = await httpclient.PostAsJsonAsync("auth/register", new UserInfo { Username = username, Password = password });
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> LogoutAsync()
    {
        var response = await httpclient.PostAsync("auth/logout", content: null);
        return response.IsSuccessStatusCode;
    }
}