using Microsoft.AspNetCore.Identity;
using NexusChat.DTOs.Request;
using NexusChat.DTOs.Response;

namespace NexusChat.Services;

public interface IUsuarioService
{
    public Task<UsuarioResponse?> BuscarPorNome(string nomeDeUsuario);
    public Task<JwtResponse?> RecriarRefreshToken(RefreshTokenRequest refreshRequest);
    public Task<JwtResponse?> Entrar(UsuarioSignRequest usuarioRequest);
    public Task<IdentityResult> Registrar(UsuarioSignRequest usuarioRequest);
}