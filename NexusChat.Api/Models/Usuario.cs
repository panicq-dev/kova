using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace NexusChat.Model;

public class Usuario : IdentityUser
{
    [Required]
    public string Id { get; set; }
    [Required(ErrorMessage = "É nessário registrar um nome de usuário")]
    [MaxLength(50, ErrorMessage = "O nome de usuário é muito grande! Por favor, diminua para menos de 50 caracteres")]
    public string NomeDeUsuario { get; set; } = string.Empty;
    [Required(ErrorMessage = "É nessário registrar um e-mail.")]
    [EmailAddress(ErrorMessage = "O formato do e-mail é inválido.")]
    public string Email { get; set; } = string.Empty;
    [Required(ErrorMessage = "Insira sua senha...")]
    [DataType(DataType.Password)]
    public string Senha { get; set; } = string.Empty;
    public string? RefreshToken { get; set; } = string.Empty;
    public DateTime RefreshTokenExpiryTime { get; set; }
}