using Application.Services.Interface;
using Domain.Models;
using Domain.Repositories.Interface;

namespace Application.Services.Class;

public sealed class UserService : IUserService
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

    public async Task<User?> Get(Guid id, CancellationToken cancellationToken)
    {
        User? user = await _userRepository.Get(id, cancellationToken);

        return user is not null ? user : null;
    }

    public async Task Create(User user, CancellationToken cancellationToken)
    {
        await _userRepository.Create(user, cancellationToken);
    }

    public async Task Update(User user, CancellationToken cancellationToken)
    {
        await _userRepository.Update(user, cancellationToken);
    }

    public async Task Delete(User user, CancellationToken cancellationToken)
    {
        await _userRepository.Delete(user, cancellationToken);
    }
}
