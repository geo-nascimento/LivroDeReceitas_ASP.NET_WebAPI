namespace MeuLivroDeReceitas.Comunication.Response;

public class RespostaErroJson
{
    public List<string>? Mensagens { get; set; }

    public RespostaErroJson(string msg)
    {
        Mensagens = new List<string>() { msg };
    }

    public RespostaErroJson(List<string> mensagens)
    {
        Mensagens = mensagens;
    }
}