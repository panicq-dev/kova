using System.ComponentModel.DataAnnotations;
using NexusChat.Api.Models;
using NexusChat.Models;

namespace NexusChat.DTOs.Response;

public class UsuarioResponse
{
    public string Id { get; set; } = string.Empty;
    public string NomeDeUsuario { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public ICollection<Amizade> PedidosEnviados { get; set; } = new List<Amizade>();
    public ICollection<Amizade> PedidosRecebidos { get; set; } = new List<Amizade>();
}