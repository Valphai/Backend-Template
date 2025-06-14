using MediatR;
using Microsoft.AspNetCore.Mvc;
using Project.Application.Users.Authentication;
using Project.Application.Users.Create;
using Project.Application.Users.RefreshToken;
using Project.Domain.Security;
using Project.WebApi.Extensions;

namespace Project.WebApi.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(IMediator mediator, Configuration configuration) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request, CancellationToken cancellation)
    {
        var response = await mediator.Send(request, cancellation);

        if (response is null) return BadRequest();

        return Ok(response);
    }
    
    [HttpPost("authentication")]
    public async Task<IActionResult> Authentication([FromBody] AuthenticationRequest request,
        CancellationToken cancellation)
    {
        var response = await mediator.Send(request, cancellation);

        if (response?.Data is null) return BadRequest();

        response.Data.Token = JwtExtension.Generate(response.Data, configuration.Secrets.JwtPrivateKey!);

        return Ok(response);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request,
        CancellationToken cancellation)
    {
        var response = await mediator.Send(request, cancellation);

        if (response?.Data is null) return BadRequest();

        response.Data.Token = JwtExtension.Generate(response.Data, configuration.Secrets.JwtPrivateKey!);
        return Ok(response);
    }
}