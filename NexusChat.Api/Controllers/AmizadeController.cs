using Microsoft.AspNetCore.Mvc;
using NexusChat.DTOs.Request;
using NexusChat.DTOs.Response;
using NexusChat.Services;

namespace NexusChat.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AmizadeController : ControllerBase
{
    private readonly AmizadeService _amizadeService;

    public AmizadeController(AmizadeService amizadeService)
    {
        _amizadeService = amizadeService;
    }

    [HttpPost("enviar-pedido")]
    [ProducesResponseType(typeof(SolicitacaoAmizadeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(SolicitacaoAmizadeResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(SolicitacaoAmizadeResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(SolicitacaoAmizadeResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(SolicitacaoAmizadeResponse), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> EnviarPedido([FromBody] EnviarPedidoAmizadeRequest request)
    {
        var resposta = await _amizadeService.EnviarPedido(request.SolicitanteId, request.DestinatarioId);

        if (!resposta.Sucesso)
        {
            return resposta.Codigo switch
            {
                "USUARIO_NAO_ENCONTRADO" => NotFound(resposta),
                "SOLICITACAO_INVALIDA" or "DADOS_INVALIDOS" => BadRequest(resposta),
                "JA_SAO_AMIGOS" or "PEDIDO_JA_ENVIADO" or "PEDIDO_RECEBIDO_PENDENTE" => Conflict(resposta),
                "USUARIO_BLOQUEADO" => StatusCode(StatusCodes.Status403Forbidden, resposta),
                _ => BadRequest(resposta)
            };
        }

        return Ok(resposta);
    }

    [HttpPost("aceitar-pedido")]
    [ProducesResponseType(typeof(SolicitacaoAmizadeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(SolicitacaoAmizadeResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(SolicitacaoAmizadeResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(SolicitacaoAmizadeResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(SolicitacaoAmizadeResponse), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AceitarPedido([FromBody] AceitarPedidoAmizadeRequest request)
    {
        var resposta = await _amizadeService.AceitarPedido(request.AmizadeId);

        if (!resposta.Sucesso)
        {
            return resposta.Codigo switch
            {
                "PEDIDO_NAO_ENCONTRADO" => NotFound(resposta),
                "DADOS_INVALIDOS" or "STATUS_INVALIDO" => BadRequest(resposta),
                "JA_ACEITO" => Conflict(resposta),
                "USUARIO_BLOQUEADO" => StatusCode(StatusCodes.Status403Forbidden, resposta),
                _ => BadRequest(resposta)
            };
        }

        return Ok(resposta);
    }

    [HttpDelete("recusar-pedido/{amizadeId}")]
    [ProducesResponseType(typeof(SolicitacaoAmizadeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(SolicitacaoAmizadeResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(SolicitacaoAmizadeResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(SolicitacaoAmizadeResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(SolicitacaoAmizadeResponse), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RecusarPedido([FromRoute] string amizadeId, [FromQuery] string destinatarioId)
    {
        var resposta = await _amizadeService.RecusarAmizade(amizadeId, destinatarioId);

        if (!resposta.Sucesso)
        {
            return resposta.Codigo switch
            {
                "PEDIDO_NAO_ENCONTRADO" => NotFound(resposta),
                "DADOS_INVALIDOS" or "STATUS_INVALIDO" => BadRequest(resposta),
                "JA_ACEITO" => Conflict(resposta),
                "ACESSO_NEGADO" or "USUARIO_BLOQUEADO" => StatusCode(StatusCodes.Status403Forbidden, resposta),
                _ => BadRequest(resposta)
            };
        }

        return Ok(resposta);
    }
}
