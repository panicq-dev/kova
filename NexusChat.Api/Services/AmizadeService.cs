using NexusChat.DTOs.Response;
using NexusChat.Enums;
using NexusChat.Models;
using NexusChat.Repositorio;

namespace NexusChat.Services;

public class AmizadeService
{
    private readonly IUsuarioRepositorio _usuarioRepositorio;
    private readonly IAmizadeRepositorio _amizadeRepositorio;

    public AmizadeService(IUsuarioRepositorio usuarioRepositorio, IAmizadeRepositorio amizadeRepositorio)
    {
        _usuarioRepositorio = usuarioRepositorio;
        _amizadeRepositorio = amizadeRepositorio;
    }

    public async Task<SolicitacaoAmizadeResponse> EnviarPedido(string solicitanteId, string destinatarioId)
    {
        if (string.IsNullOrWhiteSpace(solicitanteId) || string.IsNullOrWhiteSpace(destinatarioId))
        {
            return new SolicitacaoAmizadeResponse
            {
                Sucesso = false,
                Codigo = "DADOS_INVALIDOS",
                Mensagem = "Os identificadores de solicitante e destinatário são obrigatórios."
            };
        }

        if (solicitanteId == destinatarioId)
        {
            return new SolicitacaoAmizadeResponse
            {
                Sucesso = false,
                Codigo = "SOLICITACAO_INVALIDA",
                Mensagem = "Você não pode enviar uma solicitação para si mesmo."
            };
        }

        var solicitante = await _usuarioRepositorio.ObterPorId(solicitanteId);
        var destinatario = await _usuarioRepositorio.ObterPorId(destinatarioId);

        if (solicitante is null || destinatario is null)
        {
            return new SolicitacaoAmizadeResponse
            {
                Sucesso = false,
                Codigo = "USUARIO_NAO_ENCONTRADO",
                Mensagem = "Um ou ambos os usuários não foram encontrados."
            };
        }

        var amizadeExistente = await _amizadeRepositorio.ObterAmizadeEntreUsuarios(solicitanteId, destinatarioId);
        if (amizadeExistente is not null)
        {
            return amizadeExistente.Status switch
            {
                AmizadeStatus.Aceito => new SolicitacaoAmizadeResponse
                {
                    Sucesso = false,
                    Codigo = "JA_SAO_AMIGOS",
                    Mensagem = "Vocês já são amigos."
                },
                AmizadeStatus.Bloqueado => new SolicitacaoAmizadeResponse
                {
                    Sucesso = false,
                    Codigo = "USUARIO_BLOQUEADO",
                    Mensagem = "Não foi possível enviar a solicitação de amizade."
                },
                AmizadeStatus.Pendente when amizadeExistente.SolicitanteId == solicitanteId => new SolicitacaoAmizadeResponse
                {
                    Sucesso = false,
                    Codigo = "PEDIDO_JA_ENVIADO",
                    Mensagem = "Você já possui uma solicitação pendente para este usuário."
                },
                AmizadeStatus.Pendente => new SolicitacaoAmizadeResponse
                {
                    Sucesso = false,
                    Codigo = "PEDIDO_RECEBIDO_PENDENTE",
                    Mensagem = "Este usuário já enviou uma solicitação de amizade para você. Verifique seus pedidos recebidos."
                },
                _ => new SolicitacaoAmizadeResponse
                {
                    Sucesso = false,
                    Codigo = "STATUS_INVALIDO",
                    Mensagem = "Não foi possível processar a solicitação."
                }
            };
        }

        var novaAmizade = new Amizade
        {
            Id = Guid.NewGuid().ToString(),
            SolicitanteId = solicitanteId,
            DestinatarioId = destinatarioId,
            Status = AmizadeStatus.Pendente,
            CriadoEm = DateTime.UtcNow
        };

        await _amizadeRepositorio.CriarPedido(novaAmizade);

        return new SolicitacaoAmizadeResponse
        {
            Sucesso = true,
            Codigo = "PEDIDO_ENVIADO",
            Mensagem = "O pedido de amizade foi enviado com sucesso!"
        };
    }

    public async Task<SolicitacaoAmizadeResponse> AceitarPedido(string amizadeId)
    {
        if (string.IsNullOrWhiteSpace(amizadeId))
        {
            return new SolicitacaoAmizadeResponse
            {
                Sucesso = false,
                Codigo = "DADOS_INVALIDOS",
                Mensagem = "O identificador da solicitação é obrigatório."
            };
        }

        var amizade = await _amizadeRepositorio.ObterAmizadePorId(amizadeId);
        if (amizade is null)
        {
            return new SolicitacaoAmizadeResponse
            {
                Sucesso = false,
                Codigo = "PEDIDO_NAO_ENCONTRADO",
                Mensagem = "Solicitação de amizade não encontrada."
            };
        }

        if (amizade.Status == AmizadeStatus.Aceito)
        {
            return new SolicitacaoAmizadeResponse
            {
                Sucesso = false,
                Codigo = "JA_ACEITO",
                Mensagem = "Esta solicitação de amizade já foi aceita anteriormente."
            };
        }

        if (amizade.Status == AmizadeStatus.Bloqueado)
        {
            return new SolicitacaoAmizadeResponse
            {
                Sucesso = false,
                Codigo = "USUARIO_BLOQUEADO",
                Mensagem = "Não é possível aceitar a solicitação pois a relação está bloqueada."
            };
        }

        if (amizade.Status != AmizadeStatus.Pendente)
        {
            return new SolicitacaoAmizadeResponse
            {
                Sucesso = false,
                Codigo = "STATUS_INVALIDO",
                Mensagem = "Apenas solicitações com status pendente podem ser aceitas."
            };
        }

        amizade.Status = AmizadeStatus.Aceito;
        amizade.AtualizadoEm = DateTime.UtcNow;

        await _amizadeRepositorio.Atualizar(amizade);

        return new SolicitacaoAmizadeResponse
        {
            Sucesso = true,
            Codigo = "PEDIDO_ACEITO",
            Mensagem = "O pedido de amizade foi aceito com sucesso."
        };
    }

    public async Task<SolicitacaoAmizadeResponse> RecusarAmizade(string amizadeId, string destinatarioId)
    {
        if (string.IsNullOrWhiteSpace(amizadeId) || string.IsNullOrWhiteSpace(destinatarioId))
        {
            return new SolicitacaoAmizadeResponse
            {
                Sucesso = false,
                Codigo = "DADOS_INVALIDOS",
                Mensagem = "Os identificadores de destinatarioId e amizadeId são obrigatórios."
            };
        }

        var amizade = await _amizadeRepositorio.ObterAmizadePorId(amizadeId);
        if (amizade is null)
        {
            return new SolicitacaoAmizadeResponse
            {
                Sucesso = false,
                Codigo = "PEDIDO_NAO_ENCONTRADO",
                Mensagem = "Solicitação de amizade não encontrada."
            };
        }

        if (amizade.DestinatarioId != destinatarioId)
        {
            return new SolicitacaoAmizadeResponse
            {
                Sucesso = false,
                Codigo = "ACESSO_NEGADO",
                Mensagem = "Você não tem permissão para recusar esta solicitação."
            };
        }

        if (amizade.Status == AmizadeStatus.Aceito)
        {
            return new SolicitacaoAmizadeResponse
            {
                Sucesso = false,
                Codigo = "JA_ACEITO",
                Mensagem = "Esta solicitação já foi aceita anteriormente e não pode ser recusada."
            };
        }

        if (amizade.Status == AmizadeStatus.Bloqueado)
        {
            return new SolicitacaoAmizadeResponse
            {
                Sucesso = false,
                Codigo = "USUARIO_BLOQUEADO",
                Mensagem = "Não é possível recusar uma solicitação com relação bloqueada."
            };
        }

        if (amizade.Status != AmizadeStatus.Pendente)
        {
            return new SolicitacaoAmizadeResponse
            {
                Sucesso = false,
                Codigo = "STATUS_INVALIDO",
                Mensagem = "Apenas solicitações com status pendente podem ser recusadas."
            };
        }

        await _amizadeRepositorio.Remover(amizade);

        return new SolicitacaoAmizadeResponse
        {
            Sucesso = true,
            Codigo = "PEDIDO_RECUSADO",
            Mensagem = "O pedido de amizade foi recusado com sucesso."
        };
    }
}
