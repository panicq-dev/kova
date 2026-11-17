using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using NexusChat.DTOs.Response;
using NexusChat.Model;

namespace NexusChat.Services;

public class AutenticacaoService 
{
    private readonly IConfiguration _configuration;

    public AutenticacaoService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public JwtResponse GerarToken(Usuario usuario)
    {
        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Name, usuario.NomeDeUsuario),
            new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };
        var expiracao = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["JWTConfig:TempoExpiracaoEmMinutos"]));
        var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTKey:key"]!));
        var credenciais = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);
        
        var tokenPropriedades = new JwtSecurityToken(
            issuer: "NexusChat.Api",
            audience: "NexusChatClient",
            expires: expiracao,
            claims: claims,
            signingCredentials: credenciais
            );

        var token = new JwtSecurityTokenHandler().WriteToken(tokenPropriedades);
        var refreshToken = GerarRefreshToken(token);
        
        return new JwtResponse()
        {
            Token = token,
            RefreshToken = refreshToken
        };
    }

    public string GerarRefreshToken(string token)
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(128));
    }
}