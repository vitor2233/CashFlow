
using CashFlow.Application.UseCases.Users.Register;
using CashFlow.Application.UseCases.Users.Profile;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CashFlow.Application.UseCases.Users.Update;
using CashFlow.Application.UseCases.Users.ChangePassword;

namespace CashFlow.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisterUserJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(
        [FromServices] IRegisterUserUseCase useCase,
        [FromBody] RequestRegisterUserJson request)
    {
        var response = await useCase.Execute(request);
        return Created(string.Empty, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ResponseUserProfileJson), StatusCodes.Status200OK)]
    [Authorize]
    public async Task<IActionResult> GetProfile([FromServices] IGetUserProfileUseCase useCase)
    {
        var response = await useCase.Execute();

        return Ok(response);
    }

    [HttpPut]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateProfile([FromServices] IUpdateUserUseCase useCase,
    [FromBody] RequestUpdateUserJson request)
    {
        await useCase.Execute(request);

        return NoContent();
    }

    [HttpPatch("change-password")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ChangePassword([FromServices] IChangePasswordUseCase useCase,
    [FromBody] RequestChangePasswordJson request)
    {
        await useCase.Execute(request);

        return NoContent();
    }
}