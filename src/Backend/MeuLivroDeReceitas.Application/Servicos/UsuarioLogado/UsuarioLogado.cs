using MeuLivroDeReceitas.Application.Servicos.Token;
using MeuLivroDeReceitas.Domain.Entities;
using MeuLivroDeReceitas.Domain.Repositories;
using Microsoft.AspNetCore.Http;

namespace MeuLivroDeReceitas.Application.Servicos.UsuarioLogado;

public class UsuarioLogado : IUsuarioLogado
{
    private readonly IHttpContextAccessor _httpContextAcessor;
    private readonly TokenController _tokenController;
    private readonly IUsuarioReadOnly _repo;

    public UsuarioLogado(IHttpContextAccessor httpContextAcessor, TokenController tokenController, IUsuarioReadOnly repo)
    {
        _httpContextAcessor = httpContextAcessor; //Representa o contexto da request recebida
        _tokenController = tokenController;
        _repo = repo;
    }
    
    public async Task<Usuario> RecuperarUsuario()
    {
        var authorization = _httpContextAcessor.HttpContext.Request.Headers["Authorization"].ToString();

        var token = authorization["Bearer".Length..].Trim();

        var emailUsuario = _tokenController.RecuperarEmail(token);
        
        
        var usuario = await _repo.RecuperarPorEmail(emailUsuario);

        return usuario;
    }
}