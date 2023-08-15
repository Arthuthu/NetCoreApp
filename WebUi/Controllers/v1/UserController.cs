using Application.Services.Interface;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;


namespace WebUi.Controllers.v1;

[Route("api/[controller]")]
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
	public async Task<IActionResult> Get(CancellationToken cancellationToken)
	{
		List<User>? users = await _userService.Get(cancellationToken);

		if (users is null)
			return NotFound();

		return Ok(users);
	}

	[HttpGet]
	[Route("/getbyid/{id:guid}")]
	public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
	{
		User? user = await _userService.Get(id, cancellationToken);

		if (user is null)
			return NotFound();

		return Ok(user);
	}

	[HttpPost]
	[Route("/post")]
	public async Task<IActionResult> Post(User user, CancellationToken cancellationToken)
	{
		try
		{
			await _userService.Create(user, cancellationToken);
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
	public async Task<IActionResult> Put(User user, CancellationToken cancellationToken)
	{
		bool userWasCreated = await _userService.Update(user, cancellationToken);

		if (userWasCreated is false)
			return NotFound("User was not found");

		return Ok(user);
	}

	[HttpDelete]
	[Route("/delete/{id:guid}")]
	public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
	{
		bool userWasDeleted = await _userService.Delete(id, cancellationToken);

		if (userWasDeleted is false)
			return NotFound("User was not found");

		return Ok("User was successfully deleted");
	}
}
