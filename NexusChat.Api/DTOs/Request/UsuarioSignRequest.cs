using System.ComponentModel.DataAnnotations;

namespace NexusChat.DTOs.Request;

public class UsuarioSignRequest
{
    [Required(ErrorMessage = "É necessário registrar um nome de usuário.")]
    [MaxLength(50, ErrorMessage = "O nome de usuário é muito grande! Por favor, diminua para menos de 50 caracteres.")]
    public string NomeDeUsuario { get; set; } = string.Empty;

    [Required(ErrorMessage = "É necessário registrar um e-mail.")]
    [EmailAddress(ErrorMessage = "O formato do e-mail é inválido.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Insira sua senha.")]
    [DataType(DataType.Password)]
    public string Senha { get; set; } = string.Empty;
}