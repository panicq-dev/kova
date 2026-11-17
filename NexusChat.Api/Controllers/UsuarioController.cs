using Microsoft.AspNetCore.Mvc;
using NexusChat.DTOs;
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
    public async Task Registrar(UsuarioSignRequest usuarioSign)
    {
        return await _usuarioService.
    }
}