using Microsoft.EntityFrameworkCore;
using NexusChat.Data;
using NexusChat.Models;

namespace NexusChat.Repositorio;

public class AmizadeRepositorio : IAmizadeRepositorio
{
    private readonly AppDbContext _db;

    public AmizadeRepositorio(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Amizade?> ObterAmizadePorId(string amizadeId)
    {
        return await _db.Amizades
            .FirstOrDefaultAsync(a => a.Id == amizadeId);
    }

    public async Task<Amizade?> ObterAmizadeEntreUsuarios(string usuarioId1, string usuarioId2)
    {
        return await _db.Amizades
            .FirstOrDefaultAsync(a =>
                (a.SolicitanteId == usuarioId1 && a.DestinatarioId == usuarioId2) ||
                (a.SolicitanteId == usuarioId2 && a.DestinatarioId == usuarioId1));
    }

    public async Task CriarPedido(Amizade amizade)
    {
        await _db.Amizades.AddAsync(amizade);
        await _db.SaveChangesAsync();
    }

    public async Task Atualizar(Amizade amizade)
    {
        _db.Amizades.Update(amizade);
        await _db.SaveChangesAsync();
    }

    public async Task Remover(Amizade amizade)
    {
        _db.Amizades.Remove(amizade);
        await _db.SaveChangesAsync();
    }
}
