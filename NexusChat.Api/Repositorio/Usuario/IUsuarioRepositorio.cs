using Microsoft.AspNetCore.Identity;
using NexusChat.Api.Models;

namespace NexusChat.Repositorio;

public interface IUsuarioRepositorio
{
    Task<IdentityResult> Registrar(Usuario usuario, string senha);
    Task<Usuario?> ObterPorId(string id);
    Task<Usuario?> ObterPorNomeDeUsuario(string nomeDeUsuario);
    Task<bool> VerificarSenha(Usuario usuario, string senha);
    Task<IdentityResult> Atualizar(Usuario usuario);
}