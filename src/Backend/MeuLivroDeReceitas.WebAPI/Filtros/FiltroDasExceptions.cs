using System.Net;
using MeuLivroDeReceitas.Comunication.Response;
using MeuLivroDeReceitas.Exceptions;
using MeuLivroDeReceitas.Exceptions.ExecptionsBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MeuLivroDeReceitas.WebAPI.Filtros;

public class FiltroDasExceptions : IExceptionFilter
{
    //Árvore de possibilidades de exceções
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is MeuLivroDereceitasExceptions)
        {
            TratarMeuLivroDeReceitasException(context);//Dentro desse tipo existem nossas exeções personalizadas
        }
        else
        {
            LancarErroDesconhecido(context);
        }
    }
    
    //Trata nossas execções personalizadas: varios tipos diferentes
    private void TratarMeuLivroDeReceitasException(ExceptionContext context)
    {
        if (context.Exception is ErrosDeValidacaoException)
        {
            TratarErroDeValidacaoExecption(context);
        }
        else if (context.Exception is LoginInvalidoException)
        {
            TratarLoginException(context);
        }
        else
        {
            TratarOutraException(context);
        }
        
    }
    //TODO Fazer a implementação do tratamento da função TratarOutraException()
    private void TratarOutraException(ExceptionContext context)
    {
        throw new NotImplementedException();
    }

    private void TratarErroDeValidacaoExecption(ExceptionContext context)
    {
        var errorDeValidacaoExecption = context.Exception as ErrosDeValidacaoException;//Cast para o tipo
        context.Result = new ObjectResult(new RespostaErroJson(errorDeValidacaoExecption.MensagensDeErro));
        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
    }

    private void TratarLoginException(ExceptionContext context)
    {
        var erroLoginException = context.Exception as LoginInvalidoException;
        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        context.Result = new ObjectResult(new RespostaErroJson(erroLoginException.Message));
    }

    private void LancarErroDesconhecido(ExceptionContext context)
    {
        //Http response
        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //corpo da resposta
        context.Result = new ObjectResult(new RespostaErroJson(ResourceMensagensDeErro.ERRO_DESCONHECIDO));
    }
}