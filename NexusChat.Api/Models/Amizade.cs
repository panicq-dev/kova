using NexusChat.Api.Models;
using NexusChat.Enums;

namespace NexusChat.Models;

public class Amizade
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string SolicitanteId { get; set; } = string.Empty; 
    public Usuario Solicitante { get; set; } = null!;

    public string DestinatarioId { get; set; } = string.Empty; 
    public Usuario Destinatario { get; set; } = null!;

    public AmizadeStatus Status { get; set; } = AmizadeStatus.Pendente;

    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
    public DateTime? AtualizadoEm { get; set; }
}
