using Domain.Models;

namespace Domain.Repositories.Auth.Interface
{
    public interface IAuthRepository
    {
        Task<UserModel?> Login(UserModel user, CancellationToken cancellationToken);
    }
}