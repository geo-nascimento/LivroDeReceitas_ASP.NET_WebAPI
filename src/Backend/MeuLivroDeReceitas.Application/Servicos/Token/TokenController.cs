using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Pomelo.EntityFrameworkCore.MySql.Query.ExpressionTranslators.Internal;

namespace MeuLivroDeReceitas.Application.Servicos.Token;

public class TokenController
{
    private const string EmailAlias = "eml";
    private readonly double _tempoDeVidaEmMinutos;
    private readonly string _chaveDeSeguranca;

    public TokenController(double tempoDeVidaEmMinutos, string chaveDeSeguranca)
    {
        _tempoDeVidaEmMinutos = tempoDeVidaEmMinutos;
        _chaveDeSeguranca = chaveDeSeguranca;
    }

    public string GerarToken(string emailUsuario)
    {
        var claims = new List<Claim>
        {
            new Claim(EmailAlias, emailUsuario)
        };
        
        //Objeto de criação do token
        var tokenHandler = new JwtSecurityTokenHandler();
        
        //Descrição do token
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_tempoDeVidaEmMinutos),
            SigningCredentials = new SigningCredentials(SimeticKey(), SecurityAlgorithms.HmacSha256Signature)
        };

        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        var token = tokenHandler.WriteToken(securityToken);
        
        return token;
    }
    
    //Validar o token
    public ClaimsPrincipal ValidarToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        //Parametros de validação
        var parametrosDeValidacao = new TokenValidationParameters()
        {
            RequireExpirationTime = true,
            IssuerSigningKey = SimeticKey(),
            ClockSkew = new TimeSpan(0),
            ValidateIssuer = false,
            ValidateAudience = false
        };

        var claims = tokenHandler.ValidateToken(token, parametrosDeValidacao, out _);//out _ é pra ignorar o retorno

        return claims;
    }

    private SymmetricSecurityKey SimeticKey()
    {
        var symetricKey = Convert.FromBase64String(_chaveDeSeguranca);
        return new SymmetricSecurityKey(symetricKey);
    }

    public string RecuperarEmail(string token)
    {
        var claims = ValidarToken(token);

        return claims.FindFirst(EmailAlias).Value;
    }
}