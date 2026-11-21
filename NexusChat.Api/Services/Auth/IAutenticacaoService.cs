using NexusChat.Api.Models;
using NexusChat.DTOs.Response;

namespace NexusChat.Services;

public interface IAutenticacaoService
{
    public string GerarRefreshToken();
    public JwtResponse GerarToken(Usuario usuario);
}