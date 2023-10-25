using MeuLivroDeReceitas.Domain.Entities;
using MeuLivroDeReceitas.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MeuLivroDeReceitas.Infrastructure.AcessoRepository.Repository;

public class UsuarioRepositorio : IUsuarioReadOnly, IUsuarioWriteOnly, IUsuarioUpdateOnly
{
    private readonly LivroDeReceitasContext _db;

    public UsuarioRepositorio(LivroDeReceitasContext db)
    {
        _db = db;
    }

    public async Task<bool> ExisteUsuarioComEmail(string email)
    {
        return await _db.Usuarios.AnyAsync(c => c.Email.Equals(email));
    }

    public async Task<Usuario> Login(string email, string senha)
    {
        return await _db.Usuarios
            .AsNoTracking()//cenário somente leitura
            .FirstOrDefaultAsync(u => u.Email.Equals(email) && u.Senha.Equals(senha));
    }

    public async Task<Usuario> RecuperarPorEmail(string email)//Somente leitura
    {
        return await _db.Usuarios.AsNoTracking().FirstOrDefaultAsync(c => c.Email.Equals(email));
    }

    public async Task Adicionar(Usuario usuario)
    {
        await _db.Usuarios.AddAsync(usuario);
    }

    public void Update(Usuario usuario)
    {
        _db.Usuarios.Update(usuario);
        
    }

    public async Task<Usuario> RecuperarPorId(long id)
    {
        return await _db.Usuarios.FirstOrDefaultAsync(c => c.Id == id);
    }
}