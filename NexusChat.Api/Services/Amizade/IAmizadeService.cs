using NexusChat.DTOs.Response;

namespace NexusChat.Services;

public interface IAmizadeService
{
    public Task<SolicitacaoAmizadeResponse> RecusarAmizade(string amizadeId, string destinatarioId);
    public Task<SolicitacaoAmizadeResponse> AceitarPedido(string amizadeId);
    public Task<SolicitacaoAmizadeResponse> EnviarPedido(string solicitanteId, string destinatarioId);
}