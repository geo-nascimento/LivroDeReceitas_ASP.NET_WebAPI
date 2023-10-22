using MeuLivroDeReceitas.Application.UseCases.Login.FazerLogin;
using MeuLivroDeReceitas.Comunication.Request;
using Microsoft.AspNetCore.Mvc;

namespace MeuLivroDeReceitas.WebAPI.Controllers;

public class LoginController : MeuLivroDeReceitasController
{
    [HttpPost]
    [ProducesResponseType(typeof(RequisicaoLoginJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> Login(
        [FromServices] ILoginUseCase lguc, 
        RequisicaoLoginJson requisicao)
    {
        var resposta = await lguc.Executar(requisicao);
        return Ok(resposta);
    }
}