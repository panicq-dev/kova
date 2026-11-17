using Microsoft.AspNetCore.Mvc;
using NexusChat.DTOs;
using NexusChat.DTOs.Request;
using NexusChat.Services;

namespace NexusChat.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly UsuarioService _usuarioService;

    public UsuarioController(UsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpPost]
    public async Task<IActionResult> Registrar(UsuarioSignRequest usuarioRequest)
    {
        var resultado = await _usuarioService.Registrar(usuarioRequest);
        if (!resultado.Succeeded)
        {
            return BadRequest(resultado.Errors);
        }
        return Ok(resultado);
        
    }

    public async Task<IActionResult> Login(UsuarioSignRequest usuarioRequest)
    {
        var resposta = await _usuarioService.Entrar(usuarioRequest);
        if (resposta is null)
        {
            return Unauthorized("O usuário ou senha são invalidos.");
        };
        return Ok(resposta);
    }

    public async Task<IActionResult> RecriarRefreshToken(RefreshTokenRequest refreshTokenRequest)
    {
        var resposta = await _usuarioService.RecriarRefreshToken(refreshTokenRequest);
        if (resposta is null)
        {
            return Unauthorized("Sem autorização. RefreshToken expirado ou nulo.");
        };
        return Ok(resposta);
    }
}