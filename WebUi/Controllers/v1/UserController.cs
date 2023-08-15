using Application.Services.User.Interface;
using Domain.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebUi.Dto;
using WebUi.Mapper;

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

	[HttpGet]
	[Route("/get")]
	[ProducesResponseType(typeof(List<UserModel>), 200)]
	[ProducesResponseType(404)]
	public async Task<IActionResult> Get(CancellationToken cancellationToken)
	{
		List<UserModel>? users = await _userService.Get(cancellationToken);

		if (users is null)
			return NotFound();

		return Ok(users);
	}

	[HttpGet]
	[Route("/getbyid/{id:guid}")]
	[ProducesResponseType(typeof(UserModel), 200)]
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
		}
		catch (Exception ex)
		{
			_logger.LogError("Something went wrong when creating the user", ex);
			return BadRequest(ex.Message);
		}

		return Ok(user);
	}

	[HttpPut]
	[Route("/update")]
	[ProducesResponseType(typeof(UserModel),200)]
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
	[ProducesResponseType(404)]
	public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
	{
		bool userWasDeleted = await _userService.Delete(id, cancellationToken);

		if (userWasDeleted is false)
			return NotFound("User was not found");

		return Ok("User was successfully deleted");
	}
}
