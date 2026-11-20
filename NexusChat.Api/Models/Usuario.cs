using Microsoft.AspNetCore.Identity;
using NexusChat.Models;

namespace NexusChat.Api.Models;

public class Usuario : IdentityUser
{
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenTempoExpiracao { get; set; }
    public ICollection<Amizade> PedidosEnviados { get; set; } = new List<Amizade>();
    public ICollection<Amizade> PedidosRecebidos { get; set; } = new List<Amizade>();
}