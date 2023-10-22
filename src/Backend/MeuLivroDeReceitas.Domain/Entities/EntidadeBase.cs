namespace MeuLivroDeReceitas.Domain.Entities;

public class EntidadeBase
{
    public long Id { get; set; }
    public DateTime DtaCriacao { get; set; } = DateTime.UtcNow;
}