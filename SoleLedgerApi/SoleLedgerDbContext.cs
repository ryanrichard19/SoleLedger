using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SoleLedgerApi;

public class SoleLedgerDbContext(DbContextOptions<SoleLedgerDbContext> options) : IdentityDbContext<SoleLedgerUser>(options) 
    
{
    public DbSet<Client> Clients => Set<Client>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Client>()
            .HasOne<SoleLedgerUser>()
            .WithMany()
            .HasForeignKey(c => c.SoleLedgerUserId)
            .HasPrincipalKey(u => u.Id);

        builder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.ClientName).IsRequired();
            entity.Property(e => e.ContactEmail).IsRequired();
        });

         base.OnModelCreating(builder);
    }
}