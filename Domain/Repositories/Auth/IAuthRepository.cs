using Domain.Models;

namespace Domain.Repositories.Auth;

public interface IAuthRepository
{
    Task<UserModel?> Login(UserModel user, CancellationToken cancellationToken);
}