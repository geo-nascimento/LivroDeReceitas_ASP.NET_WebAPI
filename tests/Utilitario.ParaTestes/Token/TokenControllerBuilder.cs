using MeuLivroDeReceitas.Application.Servicos.Token;

namespace Utilitario.ParaTestes.Token;

public class TokenControllerBuilder
{
    public static TokenController Instancia()
    {
        return new TokenController(1000, "RidRNTJPYDUwJ3llfSh6TEhVKzt+QVhecQ==");
    }
}