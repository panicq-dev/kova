using System.ComponentModel.DataAnnotations;

namespace NexusChat.DTOs.Request;

public class RefreshTokenRequest
{
    [Required(ErrorMessage = "O nome de usuário é obrigatório.")]
    public string NomeDeUsuario { get; set; } = string.Empty;

    [Required(ErrorMessage = "O Refresh Token é obrigatório.")]
    public string RefreshToken { get; set; } = string.Empty;
}