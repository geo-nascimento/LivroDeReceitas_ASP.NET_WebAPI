using FluentValidation.Results;
using MeuLivroDeReceitas.Application.Servicos.Critografia;
using MeuLivroDeReceitas.Application.Servicos.UsuarioLogado;
using MeuLivroDeReceitas.Comunication.Request;
using MeuLivroDeReceitas.Domain.Repositories;
using MeuLivroDeReceitas.Exceptions;
using MeuLivroDeReceitas.Exceptions.ExecptionsBase;
using MeuLivroDeReceitas.Infrastructure.AcessoRepository.Repository;

namespace MeuLivroDeReceitas.Application.UseCases.Usuario.AlterarSenha;

public class AlterarSenhaUSeCase : IAlterarSenhaUseCase
{
    private readonly IUsuarioUpdateOnly _repo;
    private readonly IUsuarioLogado _usuarioLogado;
    private readonly EncriptadorDeSenha _encriptadorDeSenha;
    private readonly IUnitOfWork _unitOfWork;

    public AlterarSenhaUSeCase(IUsuarioUpdateOnly repo, 
        IUsuarioLogado usuarioLogado, 
        EncriptadorDeSenha encriptadorDeSenha,
        IUnitOfWork unitOfWork)
    {
        _repo = repo;
        _usuarioLogado = usuarioLogado;
        _encriptadorDeSenha = encriptadorDeSenha;
        _unitOfWork = unitOfWork;
    }
    
    
    public async Task Executar(RequestAlterarSenhaJson request)
    {
        //Recuperar o usuario por meio do token vindo do header da requisição
        var usuarioLogado = await _usuarioLogado.RecuperarUsuario();//apenas leitura

        var usuario = await _repo.RecuperarPorId(usuarioLogado.Id);//Possivel fazer alterações. Não-trackeado
        
        //Validações e criptografia de senha
        Validar(request, usuarioLogado);
        
        usuario.Senha = _encriptadorDeSenha.Criptografar(request.NovaSenha);
        
        _repo.Update(usuario);

        await _unitOfWork.Commit();
    }

    private void Validar(RequestAlterarSenhaJson request, Domain.Entities.Usuario usuario)
    {
        var validator = new AlterarSenhaValidator();
        var resultado = validator.Validate(request);
        
        //validar se a senha atual fornencida é a mesma salva no banco
        var senhaAtualCriptografada = _encriptadorDeSenha.Criptografar(request.SenhaAtual);

        if (!usuario.Senha.Equals(senhaAtualCriptografada))
        {
            resultado
                .Errors
                .Add(new ValidationFailure("senhaAtual", ResourceMensagensDeErro.SENHA_ATUAL_INVALIDA));
        }

        if (!resultado.IsValid)
        {
            var erro = resultado.Errors.Select(x => x.ErrorMessage).ToList();
            throw new ErrosDeValidacaoException(erro);
        }
    }
}