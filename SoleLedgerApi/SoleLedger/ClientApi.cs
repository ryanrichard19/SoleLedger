using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace SoleLedgerApi.SoleLedger.ClientApi;

internal static class ClientApi
{
    public static RouteGroupBuilder MapClients(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/clients");

        group.WithTags("Clients");

        // Add security requirements, all incoming requests to this API *must*
        // be authenticated with a valid user.
        group.RequireAuthorization(pb => pb.RequireCurrentUser())
             .AddOpenApiSecurity();

        // Rate limit all of the APIs
        group.RequirePerUserRateLimit();

        // Validate the parameters
        group.WithParameterValidation(typeof(Client));

        group.MapGet("/", async (SoleLedgerDbContext db, CurrentUser user) =>
        {
            return await db.Clients.Where(client => client.SoleLedgerUserId == user.Id).Select(t => t.ToDto()).AsNoTracking().ToListAsync();
        });

        group.MapGet("/{id}", async Task<Results<Ok<ClientDto>, NotFound>> (SoleLedgerDbContext db, int id, CurrentUser user) =>
        {
            return await db.Clients.FindAsync(id) switch
            {
                Client client when client.SoleLedgerUserId == user.Id || user.IsAdmin => TypedResults.Ok(client.ToDto()),
                _ => TypedResults.NotFound()
            };
        });

        group.MapPost("/", async Task<Created<ClientDto>> (SoleLedgerDbContext db, ClientDto newClient, CurrentUser user) =>
        {
            var client = new Client
            {
                ClientName = newClient.ClientName,
                ClientAddress = newClient.ClientAddress,
                ContactEmail = newClient.ContactEmail,
                ContactPerson = newClient.ContactPerson,
                ContactPhone = newClient.ContactPhone,
                SoleLedgerUserId = user.Id
            };

            db.Clients.Add(client);
            await db.SaveChangesAsync();

            return TypedResults.Created($"/clients/{client.Id}", client.ToDto());
        });

        group.MapPut("/{id}", async Task<Results<Ok, NotFound, BadRequest>> ( SoleLedgerDbContext db, int id, ClientDto client, CurrentUser user) =>
        {
            if (id != client.Id)
            {
                return TypedResults.BadRequest();
            }

            var rowsAffected = await db.Clients.Where(t => t.Id == id && (t.SoleLedgerUserId == user.Id || user.IsAdmin))
                                             .ExecuteUpdateAsync(updates =>
                                                updates.SetProperty(t => t.ClientAddress, client.ClientAddress)
                                                       .SetProperty(t => t.ContactPerson, client.ContactPerson));

            return rowsAffected == 0 ? TypedResults.NotFound() : TypedResults.Ok();
        });

        group.MapDelete("/{id}", async Task<Results<NotFound, Ok>> (SoleLedgerDbContext db, int id, CurrentUser user) =>
        {
            var rowsAffected = await db.Clients.Where(t => t.Id == id && (t.SoleLedgerUserId == user.Id || user.IsAdmin))
                                             .ExecuteDeleteAsync();

            return rowsAffected == 0 ? TypedResults.NotFound() : TypedResults.Ok();
        });

        return group;
    }
}
