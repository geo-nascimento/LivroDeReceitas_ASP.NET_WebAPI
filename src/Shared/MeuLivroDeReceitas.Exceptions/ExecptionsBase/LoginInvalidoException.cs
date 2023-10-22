namespace MeuLivroDeReceitas.Exceptions.ExecptionsBase;

public class LoginInvalidoException : MeuLivroDereceitasExceptions
{
    public LoginInvalidoException() : base(ResourceMensagensDeErro.LOGIN_INVALIDO)
    {
        
    }
}