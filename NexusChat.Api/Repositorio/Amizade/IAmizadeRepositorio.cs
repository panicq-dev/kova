using NexusChat.Models;

namespace NexusChat.Repositorio;

public interface IAmizadeRepositorio
{
    Task<Amizade?> ObterAmizadePorId(string amizadeId);
    Task<Amizade?> ObterAmizadeEntreUsuarios(string usuarioId1, string usuarioId2);
    Task CriarPedido(Amizade amizade);
    Task Atualizar(Amizade amizade);
}
