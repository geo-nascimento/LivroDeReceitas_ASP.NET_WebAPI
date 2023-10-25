using MeuLivroDeReceitas.Application.Servicos.Token;
using MeuLivroDeReceitas.Comunication.Response;
using MeuLivroDeReceitas.Domain.Repositories;
using MeuLivroDeReceitas.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;

namespace MeuLivroDeReceitas.WebAPI.Filtros;

public class UsuarioAutenticadoAttribute : AuthorizeAttribute ,IAsyncAuthorizationFilter
{
    private readonly TokenController _tokenController;
    private readonly IUsuarioReadOnly _usuarioReadOnly;

    public UsuarioAutenticadoAttribute(TokenController tokenController, IUsuarioReadOnly usuarioReadOnly)
    {
        _tokenController = tokenController;
        _usuarioReadOnly = usuarioReadOnly;
    }
    
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        try
        {
            var token = TokenNaRequisicao(context);
            var email = _tokenController.RecuperarEmail(token);

            //Recuperar o usuário
            var usuario = await _usuarioReadOnly.RecuperarPorEmail(email);

            if (usuario is null)
            {
                throw new Exception();
            }
        }
        catch (SecurityTokenExpiredException)
        {
            TokenExpirado(context);
        }
        catch
        {
            UsuarioSemPermissao(context);
        }
    }

    private string TokenNaRequisicao(AuthorizationFilterContext context)
    {
        var authorization = context.HttpContext.Request.Headers["Authorization"].ToString();

        if (string.IsNullOrWhiteSpace(authorization))
        {
            throw new Exception();
        }

        var token = authorization["Bearer".Length..].Trim();

        return token;
    }

    private void TokenExpirado(AuthorizationFilterContext context)
    {
        context.Result = new UnauthorizedObjectResult(new RespostaErroJson(ResourceMensagensDeErro.TOKEN_EXPIRADO));
    }
    
    private void UsuarioSemPermissao(AuthorizationFilterContext context)
    {
        context.Result = new UnauthorizedObjectResult(new RespostaErroJson(ResourceMensagensDeErro.USUARIO_SEM_PERMISSAO));
    }
}