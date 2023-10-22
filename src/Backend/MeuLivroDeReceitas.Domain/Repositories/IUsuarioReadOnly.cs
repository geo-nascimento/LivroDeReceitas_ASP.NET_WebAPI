using MeuLivroDeReceitas.Domain.Entities;

namespace MeuLivroDeReceitas.Domain.Repositories;

public interface IUsuarioReadOnly
{
    Task<bool> ExisteUsuarioComEmail(string email);
    Task<Usuario> Login(string email, string senha);
}