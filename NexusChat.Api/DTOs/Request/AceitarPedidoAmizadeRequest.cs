using System.ComponentModel.DataAnnotations;

namespace NexusChat.DTOs.Request;

public class AceitarPedidoAmizadeRequest
{
    [Required(ErrorMessage = "O identificador da solicitação é obrigatório.")]
    public string AmizadeId { get; set; } = string.Empty;
}
