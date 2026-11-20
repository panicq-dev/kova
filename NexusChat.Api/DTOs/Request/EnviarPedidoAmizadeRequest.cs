using System.ComponentModel.DataAnnotations;

namespace NexusChat.DTOs.Request;

public class EnviarPedidoAmizadeRequest
{
    [Required(ErrorMessage = "O ID do solicitante é obrigatório.")]
    public string SolicitanteId { get; set; } = string.Empty;

    [Required(ErrorMessage = "O ID do destinatário é obrigatório.")]
    public string DestinatarioId { get; set; } = string.Empty;
}
