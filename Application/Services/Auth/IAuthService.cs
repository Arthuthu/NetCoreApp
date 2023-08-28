using Domain.Models;

namespace Application.Services.Auth
{
    public interface IAuthService
    {
        Task<string?> Login(UserModel user, CancellationToken cancellationToken);
    }
}