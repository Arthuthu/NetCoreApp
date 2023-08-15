using Domain.Models;
using Domain.Repositories.Interface;

namespace Application.Services.Class;

public sealed class UserService
{
	private readonly IUserRepository _userRepository;

	public UserService(IUserRepository userRepository)
	{
		_userRepository = userRepository;
	}

	public async Task<List<User>?> Get(CancellationToken cancellationToken)
	{
		List<User>? users = await _userRepository.Get(cancellationToken);

		return users is not null ? users : null;
	}
}
