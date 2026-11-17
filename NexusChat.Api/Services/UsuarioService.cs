using Microsoft.AspNetCore.Identity;
using NexusChat.DTOs;
using NexusChat.Model;
using NexusChat.Repositorio;

namespace NexusChat.Services;

public class UsuarioService
{
    private readonly AutenticacaoService _autenticacaoService;
    private readonly IUsuarioRepositorio _usuarioRepositorio;

    public UsuarioService(AutenticacaoService autenticacaoService, IUsuarioRepositorio usuarioRepositorio)
    {
        _autenticacaoService = autenticacaoService;
        _usuarioRepositorio = usuarioRepositorio;
    }

    public async Task<IdentityResult> Registrar(UsuarioSignRequest usuarioRequest)
    {
        var usuario = new Usuario()
        {
            NomeDeUsuario = usuarioRequest.NomeDeUsuario,
            Email = usuarioRequest.Email,
            Senha = usuarioRequest.Senha,
        };
        return await _usuarioRepositorio.Registrar(usuario);
    }
}