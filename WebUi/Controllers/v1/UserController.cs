using Application.Services.User;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using WebUi.Caching;
using WebUi.Dto;
using WebUi.Mapper.User;

namespace WebUi.Controllers.v1;

[Route("api/v1/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
	private readonly IUserService _userService;
	private readonly ILogger<UserController> _logger;
	private readonly IDistributedCache _cache;

	public UserController(IUserService userService,
		ILogger<UserController> logger,
		IDistributedCache cache)
	{
		_userService = userService;
		_logger = logger;
		_cache = cache;
	}

	[HttpGet, Route("/get"), Authorize(Roles = "User")]
	[ProducesResponseType(typeof(List<UserModel>), 200),
	ProducesResponseType(401),
	ProducesResponseType(404)]
	public async Task<IActionResult> Get(CancellationToken cancellationToken)
	{
		var cachedUsers = await _cache.GetRecordAsync<List<UserModel>>("users");

		if (cachedUsers is not null)
			return Ok(cachedUsers);

		List<UserModel>? users = await _userService.Get(cancellationToken);

		if (users is null)
			return NotFound();

		await _cache.SetRecordAsync("users", users, cancellationToken);

		return Ok(users);
	}

	[Authorize, HttpGet, Route("/getbyid/{id:guid}")]
	[ProducesResponseType(typeof(UserModel), 200),
	ProducesResponseType(401),
	ProducesResponseType(404)]
	public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
	{
		var cachedUser = await _cache.GetRecordAsync<UserModel>("user");

		if (cachedUser is not null)
			return Ok(cachedUser);

		UserModel? user = await _userService.Get(id, cancellationToken);

		if (user is null)
			return NotFound();

		await _cache.SetRecordAsync("user", user, cancellationToken);

		return Ok(user);
	}

	[HttpPost, Route("/post")]
	[ProducesResponseType( 200), ProducesResponseType(400)]
	public async Task<IActionResult> Post(UserRequest user, CancellationToken cancellationToken)
	{
		try
		{
			await _userService.Create(user.MapDtoToDomain(), cancellationToken);
			await _cache.RemoveAsync("users", cancellationToken);
			_logger.LogInformation("User {@Username} was successfully created", user.Username);

		}
		catch (Exception ex)
		{
			_logger.LogError("An error ocurred when creating the user", ex);
			return BadRequest(ex.Message);
		}

		return Ok(user);
	}

	[HttpPut, Route("/update")]
	[ProducesResponseType(typeof(UserModel),200),
	ProducesResponseType(401),
	ProducesResponseType(404)]
	public async Task<IActionResult> Put(UserModel user, CancellationToken cancellationToken)
	{
		bool userWasCreated = await _userService.Update(user, cancellationToken);

		if (userWasCreated is false)
			return NotFound("User was not found");

		await _cache.RemoveAsync("users", cancellationToken);
		_logger.LogInformation("User {@Username} was successfully updated", user.Username);

		return Ok(user);
	}

	[HttpDelete, Route("/delete/{id:guid}")]
	[ProducesResponseType(200),
	ProducesResponseType(401),
	ProducesResponseType(404)]
	public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
	{
		bool userWasDeleted = await _userService.Delete(id, cancellationToken);

		if (userWasDeleted is false)
			return NotFound("User was not found");

		await _cache.RemoveAsync("users", cancellationToken);

		return Ok("User was successfully deleted");
	}
}
