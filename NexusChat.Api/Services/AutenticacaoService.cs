using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using NexusChat.Api.Models;
using NexusChat.DTOs.Response;

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
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, usuario.Id),
            new Claim(JwtRegisteredClaimNames.Name, usuario.UserName ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Email, usuario.Email ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var minutosExpiracao = _configuration.GetValue<int>("JWTConfig:TempoExpiracaoEmMinutos", 30);
        var expiracao = DateTime.UtcNow.AddMinutes(minutosExpiracao);

        var jwtKey = _configuration["JWTKey:Key"]
            ?? throw new InvalidOperationException("Chave JWT (JWTKey:Key) não configurada.");

        var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var credenciais = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

        var tokenPropriedades = new JwtSecurityToken(
            issuer: _configuration["JWTConfig:Issuer"],
            audience: _configuration["JWTConfig:Audience"],
            expires: expiracao,
            claims: claims,
            signingCredentials: credenciais
        );

        var token = new JwtSecurityTokenHandler().WriteToken(tokenPropriedades);
        var refreshToken = GerarRefreshToken();

        return new JwtResponse
        {
            Token = token,
            RefreshToken = refreshToken
        };
    }

    public string GerarRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }
}