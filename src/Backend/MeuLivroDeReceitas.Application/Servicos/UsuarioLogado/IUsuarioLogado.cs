using MeuLivroDeReceitas.Domain.Entities;

namespace MeuLivroDeReceitas.Application.Servicos.UsuarioLogado;

public interface IUsuarioLogado
{
    Task<Usuario> RecuperarUsuario();
}