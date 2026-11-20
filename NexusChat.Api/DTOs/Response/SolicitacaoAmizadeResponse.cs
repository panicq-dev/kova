namespace NexusChat.DTOs.Response;

public class SolicitacaoAmizadeResponse
{
    public bool Sucesso { get; set; }
    public string Mensagem { get; set; } = string.Empty;
    public string? Codigo { get; set; }
}
