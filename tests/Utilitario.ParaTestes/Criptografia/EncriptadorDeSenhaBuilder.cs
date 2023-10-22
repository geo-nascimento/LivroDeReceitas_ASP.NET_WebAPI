using MeuLivroDeReceitas.Application.Servicos.Critografia;

namespace Utilitario.ParaTestes.Criptografia;

public class EncriptadorDeSenhaBuilder
{
    public static EncriptadorDeSenha Instancia()
    {
        return new EncriptadorDeSenha("ABCD123");
    }
}