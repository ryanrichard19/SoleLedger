## SoleLedger application with ASP.NET Core

## Prerequisites

### .NET
1. [Install .NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

### Database
1. Install the **dotnet-ef** tool: `dotnet tool install dotnet-ef -g`
1. Navigate to the `SoleLedgerApi` folder.
    1. Run `mkdir .db` to create the local database folder.
    1. Run `dotnet ef database update` to create the database.
1. Learn more about [dotnet-ef](https://learn.microsoft.com/en-us/ef/core/cli/dotnet)