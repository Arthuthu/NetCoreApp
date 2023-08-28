using Application.Services.User;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebUi.Dto;
using WebUi.Mapper.User;

namespace WebUi.Controllers.v1;

[Route("api/v1/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
	private readonly IUserService _userService;
	private readonly ILogger<UserController> _logger;

	public UserController(IUserService userService, ILogger<UserController> logger)
	{
		_userService = userService;
		_logger = logger;
	}

	[Authorize(Roles = "User")]
	[HttpGet]
	[Route("/get")]
	[ProducesResponseType(typeof(List<UserModel>), 200)]
	[ProducesResponseType(401)]
	[ProducesResponseType(404)]
	public async Task<IActionResult> Get(CancellationToken cancellationToken)
	{
		List<UserModel>? users = await _userService.Get(cancellationToken);

		if (users is null)
			return NotFound();

		return Ok(users);
	}

	[Authorize]
	[HttpGet]
	[Route("/getbyid/{id:guid}")]
	[ProducesResponseType(typeof(UserModel), 200)]
	[ProducesResponseType(401)]
	[ProducesResponseType(404)]
	public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
	{
		UserModel? user = await _userService.Get(id, cancellationToken);

		if (user is null)
			return NotFound();

		return Ok(user);
	}

	[HttpPost]
	[Route("/post")]
	[ProducesResponseType( 200)]
	[ProducesResponseType(400)]
	public async Task<IActionResult> Post(UserRequest user, CancellationToken cancellationToken)
	{
		try
		{
			await _userService.Create(user.MapDtoToDomain(), cancellationToken);
			_logger.LogInformation("User {@Username} was successfully created", user.Username);

		}
		catch (Exception ex)
		{
			_logger.LogError(ex.Message, ex);
			return BadRequest(ex.Message);
		}

		return Ok(user);
	}

	[HttpPut]
	[Route("/update")]
	[ProducesResponseType(typeof(UserModel),200)]
	[ProducesResponseType(401)]
	[ProducesResponseType(404)]
	public async Task<IActionResult> Put(UserModel user, CancellationToken cancellationToken)
	{
		bool userWasCreated = await _userService.Update(user, cancellationToken);

		if (userWasCreated is false)
			return NotFound("User was not found");

		return Ok(user);
	}

	[HttpDelete]
	[Route("/delete/{id:guid}")]
	[ProducesResponseType(200)]
	[ProducesResponseType(401)]
	[ProducesResponseType(404)]
	public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
	{
		bool userWasDeleted = await _userService.Delete(id, cancellationToken);

		if (userWasDeleted is false)
			return NotFound("User was not found");

		return Ok("User was successfully deleted");
	}
}
