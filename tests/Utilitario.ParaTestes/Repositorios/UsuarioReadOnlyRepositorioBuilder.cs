using System.Runtime.CompilerServices;
using MeuLivroDeReceitas.Domain.Entities;
using MeuLivroDeReceitas.Domain.Repositories;
using Moq;

namespace Utilitario.ParaTestes.Repositorios;

public class UsuarioReadOnlyRepositorioBuilder
{
    private static UsuarioReadOnlyRepositorioBuilder _instance;
    private readonly Mock<IUsuarioReadOnly> _repositorio;

    private UsuarioReadOnlyRepositorioBuilder()
    {
        if (_repositorio == null)
        {
            _repositorio = new Mock<IUsuarioReadOnly>();
        }
    }

    public static UsuarioReadOnlyRepositorioBuilder Instancia()
    {
        _instance = new UsuarioReadOnlyRepositorioBuilder();
        return _instance;
    }
    
    //Esse método acrescenta informções a execução da instancia desse builder
    public UsuarioReadOnlyRepositorioBuilder ExisteUsuarioComEmail(string email)
    {   
        if (!string.IsNullOrEmpty(email))
            _repositorio.Setup(i => i.ExisteUsuarioComEmail(email)).ReturnsAsync(true);
        
        return this;
    }

    public UsuarioReadOnlyRepositorioBuilder RecuperarPorEmailSenha(Usuario usuario)
    {
        _repositorio.Setup(i => i.Login(usuario.Email, usuario.Senha)).ReturnsAsync(usuario);
        
        return this;
    }
    
    public IUsuarioReadOnly Construir()
    {
        return _repositorio.Object;
    }
}