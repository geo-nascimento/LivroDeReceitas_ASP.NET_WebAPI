namespace MeuLivroDeReceitas.Exceptions.ExecptionsBase;

public class ErrosDeValidacaoException : MeuLivroDereceitasExceptions
{
    public List<string>? MensagensDeErro { get; set; }

    public ErrosDeValidacaoException(List<string> mensagensDeErro) : base (string.Empty)
    {
        MensagensDeErro = mensagensDeErro;
    }

    
}