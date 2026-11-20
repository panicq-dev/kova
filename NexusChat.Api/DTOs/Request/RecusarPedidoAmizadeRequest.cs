using System.ComponentModel.DataAnnotations;

namespace NexusChat.DTOs.Request;

public class RecusarPedidoAmizadeRequest
{
    [Required(ErrorMessage = "O identificador da solicitação é obrigatório.")]
    public string AmizadeId { get; set; } = string.Empty;

    [Required(ErrorMessage = "O identificador do destinatário é obrigatório.")]
    public string DestinatarioId { get; set; } = string.Empty;
}
