using Domain.Models;

namespace Application.Services.Auth.Interface
{
    public interface IAuthService
    {
        Task<string?> Login(UserModel user, CancellationToken cancellationToken);
    }
}