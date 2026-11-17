using Microsoft.AspNetCore.Identity;
using NexusChat.Api.Models;

namespace NexusChat.Repositorio;

public class UsuarioRepositorio : IUsuarioRepositorio
{
    private readonly UserManager<Usuario> _userManager;

    public UsuarioRepositorio(UserManager<Usuario> userManager)
    {
        _userManager = userManager;
    }

    public Task<IdentityResult> Registrar(Usuario usuario, string senha)
    {
        return _userManager.CreateAsync(usuario, senha);
    }

    public Task<Usuario?> ObterPorId(string id)
    {
        return _userManager.FindByIdAsync(id);
    }

    public Task<Usuario?> ObterPorNomeDeUsuario(string nomeDeUsuario)
    {
        return _userManager.FindByNameAsync(nomeDeUsuario);
    }

    public Task<bool> VerificarSenha(Usuario usuario, string senha)
    {
        return _userManager.CheckPasswordAsync(usuario, senha);
    }

    public Task<IdentityResult> Atualizar(Usuario usuario)
    {
        return _userManager.UpdateAsync(usuario);
    }
}