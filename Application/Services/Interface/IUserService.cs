using Domain.Models;

namespace Application.Services.Interface
{
    public interface IUserService
    {
        Task Create(User user, CancellationToken cancellationToken);
        Task Delete(User user, CancellationToken cancellationToken);
        Task<List<User>?> Get(CancellationToken cancellationToken);
        Task<User?> Get(Guid id, CancellationToken cancellationToken);
        Task Update(User user, CancellationToken cancellationToken);
    }
}