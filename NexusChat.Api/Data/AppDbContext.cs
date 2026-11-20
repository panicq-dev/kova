using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using NexusChat.Api.Models;
using NexusChat.Models;

namespace NexusChat.Data;

public class AppDbContext :  IdentityDbContext<Usuario>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<Amizade> Amizades { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Amizade>(entity =>
        {
            // Solicitante
            entity.HasOne(f => f.Solicitante)
                .WithMany(u => u.PedidosEnviados)
                .HasForeignKey(f => f.SolicitanteId)
                .OnDelete(DeleteBehavior.Restrict); 

            // Destinatario
            entity.HasOne(f => f.Destinatario)
                .WithMany(u => u.PedidosRecebidos)
                .HasForeignKey(f => f.DestinatarioId)
                .OnDelete(DeleteBehavior.Restrict);

            
            entity.HasIndex(f => new { f.DestinatarioId, f.SolicitanteId })
                .IsUnique();
        });
    }
}