using Application.Services.User.Interface;
using Domain.Models;
using Domain.Repositories.User;

namespace Application.Services.User.Class;

public sealed class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<List<UserModel>?> Get(CancellationToken cancellationToken)
    {
        List<UserModel>? users = await _userRepository.Get(cancellationToken);

        return users is not null ? users : null;
    }

    public async Task<UserModel?> Get(Guid id, CancellationToken cancellationToken)
    {
        UserModel? user = await _userRepository.Get(id, cancellationToken);

        return user is not null ? user : null;
    }

    public async Task Create(UserModel user, CancellationToken cancellationToken)
    {
        user.Id = Guid.NewGuid();

        await _userRepository.Create(user, cancellationToken);
    }

    public async Task<bool> Update(UserModel user, CancellationToken cancellationToken)
    {
        return await _userRepository.Update(user, cancellationToken);
    }

    public async Task<bool> Delete(Guid id, CancellationToken cancellationToken)
    {
        return await _userRepository.Delete(id, cancellationToken);
    }
}
