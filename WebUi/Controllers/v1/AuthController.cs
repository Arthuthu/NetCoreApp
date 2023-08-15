using Application.Services.Auth.Interface;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUi.Controllers.v1;

[Route("api/v1/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
	private readonly IAuthService _authService;

	public AuthController(IAuthService authService)
	{
		_authService = authService;
	}

	[AllowAnonymous]
	[HttpPost]
	[Route("/login")]
	[ProducesResponseType(typeof(string), 200)]
	[ProducesResponseType(404)]
	public async Task<IActionResult> Login(UserModel user, CancellationToken cancellationToken)
	{
		string? token = await _authService.Login(user, cancellationToken);

		if (token is null)
			return NotFound("Username or password is incorrect");

		return Ok(token);
	}


}
