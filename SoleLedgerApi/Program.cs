using Microsoft.AspNetCore.Identity;
using SoleLedgerApi;
using SoleLedgerApi.SoleLedger.ClientApi;

var builder = WebApplication.CreateBuilder(args);

// Configure auth
builder.Services.AddAuthentication().AddIdentityBearerToken<SoleLedgerUser>();
builder.Services.AddAuthorizationBuilder().AddCurrentUserHandler();

// Configure identity
builder.Services.AddIdentityCore<SoleLedgerUser>()
                .AddEntityFrameworkStores<SoleLedgerDbContext>()
                .AddApiEndpoints();

// Configure the database
var connectionString = builder.Configuration.GetConnectionString("SoleLedgers") ?? "Data Source=.db/SoleLedgers.db";
builder.Services.AddSqlite<SoleLedgerDbContext>(connectionString);

// State that represents the current user from the database *and* the request
builder.Services.AddCurrentUser();

// Configure Open API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o => o.AddOpenApiSecurity());

// Configure rate limiting
builder.Services.AddRateLimiting();

// Configure OpenTelemetry
builder.AddOpenTelemetry();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}




app.Map("/", () => Results.Redirect("/swagger"));

// Configure the APIs
app.MapClients();
app.MapUsers();

app.Run();
