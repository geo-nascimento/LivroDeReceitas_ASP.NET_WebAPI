using AutoMapper;
using FluentValidation.Results;
using MeuLivroDeReceitas.Application.Servicos.Critografia;
using MeuLivroDeReceitas.Application.Servicos.Token;
using MeuLivroDeReceitas.Comunication.Request;
using MeuLivroDeReceitas.Comunication.Response;
using MeuLivroDeReceitas.Domain.Repositories;
using MeuLivroDeReceitas.Exceptions;
using MeuLivroDeReceitas.Exceptions.ExecptionsBase;
using MeuLivroDeReceitas.Infrastructure.AcessoRepository.Repository;

namespace MeuLivroDeReceitas.Application.UseCases.Usuario.Registrar;

public class RegistrarUsuarioUseCase : IRegistrarUsuarioUseCase
{
    private readonly IUsuarioReadOnly _readOnly;
    private readonly IUsuarioWriteOnly _repoUsuario;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _uof;
    private readonly EncriptadorDeSenha _cripSenha;
    private readonly TokenController _tokenController;
    
    
    public RegistrarUsuarioUseCase(IUsuarioWriteOnly repositorio, IMapper mapper, IUnitOfWork uof, 
        EncriptadorDeSenha cripSenha,
        TokenController tokenController, 
        IUsuarioReadOnly readOnly)
    {
        _repoUsuario = repositorio;
        _readOnly = readOnly;
        _mapper = mapper;
        _uof = uof;
        _cripSenha = cripSenha;
        _tokenController = tokenController;
    }
    
    public async Task<ResponseUsuarioRegistrar> Executar(RequestRegistrasUsuarioJson request)//Seria o DTO de cadastro
    {
        await Validar(request);

        var entidade = _mapper.Map<Domain.Entities.Usuario>(request);
        
        entidade.Senha = _cripSenha.Criptografar(request.Senha);
        await _repoUsuario.Adicionar(entidade);
        await _uof.Commit();
        
        var token = _tokenController.GerarToken(entidade.Email);

        return new ResponseUsuarioRegistrar() { Token = token };

    }

    private async Task Validar(RequestRegistrasUsuarioJson request)
    {
        var validator = new RegistrarUsuarioValidator();
        var resultado = validator.Validate(request);

        var existeUsuarioComEmail = await _readOnly.ExisteUsuarioComEmail(request.Email);
        if (existeUsuarioComEmail)
        {
            resultado.Errors.Add(new ValidationFailure("email", ResourceMensagensDeErro.EMAIL_JA_CADASTRADO));
        }

        if (!resultado.IsValid)
        {
            var erroMsg = resultado.Errors.Select(c => c.ErrorMessage).ToList();
            throw new ErrosDeValidacaoException(erroMsg);
        }

    }
}
