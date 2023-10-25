using MeuLivroDeReceitas.Domain.Entities;

namespace MeuLivroDeReceitas.Domain.Repositories;

public interface IUsuarioUpdateOnly
{
    void Update(Usuario usuario);
    Task<Usuario> RecuperarPorId(long id);
}