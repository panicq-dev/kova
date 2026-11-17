using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NexusChat.Data;
using NexusChat.DTOs;
using NexusChat.DTOs.Response;
using NexusChat.Model;
using NexusChat.Services;

namespace NexusChat.Repositorio;

public class UsuarioRepositorio : IUsuarioRepositorio
{
    private readonly AutenticacaoService _authService;
    private readonly SignInManager<Usuario> _signInManager;
    private readonly UserManager<Usuario> _userManager;
    

    public UsuarioRepositorio(AppDbContext db, SignInManager<Usuario> signInManager, UserManager<Usuario> userManager, AutenticacaoService authService)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _authService = authService;
    }

    public async Task<IdentityResult> Registrar(Usuario usuario)
    {
        var resultado = await _userManager.CreateAsync(usuario, usuario.Senha);
        return resultado;
    }
    
    public async Task<Usuario?> ObterPorId(string id)
    {
        var usuario = await _userManager.FindByIdAsync(id);
        return usuario;
    }

    public async Task<(SignInResult Resultado, JwtResponse? Tokens)> Entrar(UsuarioSignRequest dto)
    {
        var resultadoLogin = await _signInManager.PasswordSignInAsync(
            dto.NomeDeUsuario, 
            dto.Senha, 
            isPersistent: true, 
            lockoutOnFailure: false);

        if (!resultadoLogin.Succeeded)
        {
            return (resultadoLogin, null);
        }

        var usuario = await _userManager.FindByNameAsync(dto.NomeDeUsuario);
        var parTokens = _authService.GerarToken(usuario!);

        return (resultadoLogin, parTokens);
    }
}