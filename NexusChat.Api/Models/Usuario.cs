using Microsoft.AspNetCore.Identity;

namespace NexusChat.Api.Models;

public class Usuario : IdentityUser
{
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenTempoExpiracao { get; set; }
}