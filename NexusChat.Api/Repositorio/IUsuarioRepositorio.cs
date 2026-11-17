using Microsoft.AspNetCore.Identity;
using NexusChat.DTOs;
using NexusChat.Model;

namespace NexusChat.Repositorio;

public interface IUsuarioRepositorio
{
    public Task<IdentityResult> Registrar(Usuario usuario);
}