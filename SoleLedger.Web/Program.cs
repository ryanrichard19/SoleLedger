using SoleLedger.Web;

using SoleLedger.Web.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents();

var soleLedgerUrl = builder.Configuration["SoleLedgerApi"] ??
              throw new InvalidOperationException("Todo API URL is not configured");

builder.Services.AddHttpClient<SoleLedgerClient>(client =>
{
    client.BaseAddress = new Uri(soleLedgerUrl);

    // The cookie auth stack detects this header and avoids redirects for unauthenticated
    // requests
    client.DefaultRequestHeaders.TryAddWithoutValidation("X-Requested-With", "XMLHttpRequest");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.MapRazorComponents<App>();

app.Run();
