using MeuLivroDeReceitas.Application.Servicos.UsuarioLogado;
using MeuLivroDeReceitas.Domain.Entities;
using MeuLivroDeReceitas.Domain.Repositories;
using Moq;
using Utilitario.ParaTestes.Repositorios;

namespace Utilitario.ParaTestes.UsuarioLogado;

public class UsuarioLogadoBuilder
{
    private static UsuarioLogadoBuilder _instance;
    private readonly Mock<IUsuarioLogado> _repositorio;

    private UsuarioLogadoBuilder()
    {
        if (_repositorio == null)
        {
            _repositorio = new Mock<IUsuarioLogado>();
        }
    }

    public static UsuarioLogadoBuilder Instancia()
    {
        _instance = new UsuarioLogadoBuilder();
        return _instance;
    }

    public UsuarioLogadoBuilder RecuperarUsuario(Usuario usuario)
    {
        _repositorio.Setup(c => c.RecuperarUsuario()).ReturnsAsync(usuario);

        return this;
    }
    
    public IUsuarioLogado Construir()
    {
        return _repositorio.Object;
    }
}