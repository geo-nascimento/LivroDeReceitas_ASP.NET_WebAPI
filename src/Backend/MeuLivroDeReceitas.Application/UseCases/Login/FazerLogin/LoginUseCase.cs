using MeuLivroDeReceitas.Application.Servicos.Critografia;
using MeuLivroDeReceitas.Application.Servicos.Token;
using MeuLivroDeReceitas.Comunication.Request;
using MeuLivroDeReceitas.Comunication.Response;
using MeuLivroDeReceitas.Domain.Repositories;
using MeuLivroDeReceitas.Exceptions.ExecptionsBase;

namespace MeuLivroDeReceitas.Application.UseCases.Login.FazerLogin;

public class LoginUseCase : ILoginUseCase
{
    
    private readonly IUsuarioReadOnly _repo;
    private readonly EncriptadorDeSenha _ecp;
    private readonly TokenController _tcl;

    public LoginUseCase(IUsuarioReadOnly repo, EncriptadorDeSenha ecp, TokenController tcl)
    {
        _repo = repo;
        _ecp = ecp;
        _tcl = tcl;
    }
    
    public async Task<RespostaLoginJson> Executar(RequisicaoLoginJson request)
    {
        //precisamos tratar a senha enviado pois no banco ela está criptografas
        var senhaCriptografada = _ecp.Criptografar(request.Senha);
        var usuario = await _repo.Login(request.Email,senhaCriptografada);
        if (usuario == null)
        {
            throw new LoginInvalidoException();//Exception customizada
        }

        return new RespostaLoginJson()
        {
            Nome = usuario.Nome,
            Token = _tcl.GerarToken(usuario.Email)
        };
    }
}