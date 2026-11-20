using Microsoft.AspNetCore.Mvc;
using NexusChat.Models;

namespace NexusChat.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AmizadeController
{
    [HttpPost("enviar-pedido")]
    public void EnviarPedido(Amizade amizade)
    {
        
    }
}