using Microsoft.AspNetCore.Identity;
using NexusChat.Api.Models;
using NexusChat.DTOs.Request;
using NexusChat.DTOs.Response;
using NexusChat.Repositorio;

namespace NexusChat.Services;

public class UsuarioService
{
    private readonly AutenticacaoService _autenticacaoService;
    private readonly IUsuarioRepositorio _usuarioRepositorio;
    private readonly IConfiguration _configuration;

    public UsuarioService(
        AutenticacaoService autenticacaoService,
        IUsuarioRepositorio usuarioRepositorio,
        IConfiguration configuration)
    {
        _autenticacaoService = autenticacaoService;
        _usuarioRepositorio = usuarioRepositorio;
        _configuration = configuration;
    }

    public async Task<IdentityResult> Registrar(UsuarioSignRequest usuarioRequest)
    {
        var usuario = new Usuario
        {
            UserName = usuarioRequest.NomeDeUsuario,
            Email = usuarioRequest.Email
        };

        return await _usuarioRepositorio.Registrar(usuario, usuarioRequest.Senha);
    }

    public async Task<JwtResponse?> Entrar(UsuarioSignRequest usuarioRequest)
    {
        var usuario = await _usuarioRepositorio.ObterPorNomeDeUsuario(usuarioRequest.NomeDeUsuario);
        if (usuario is null)
            return null;

        var senhaValida = await _usuarioRepositorio.VerificarSenha(usuario, usuarioRequest.Senha);
        if (!senhaValida)
            return null;

        var parTokens = _autenticacaoService.GerarToken(usuario);

        var refreshMinutos = _configuration.GetValue<int>("JWTConfig:RefreshTempoExpiracaoEmMinutos", 120);

        usuario.RefreshToken = parTokens.RefreshToken;
        usuario.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(refreshMinutos);

        await _usuarioRepositorio.Atualizar(usuario);

        return parTokens;
    }
}