using Domain.Models;

namespace Application.Services.Interface
{
    public interface IUserService
    {
		Task<List<User>?> Get(CancellationToken cancellationToken);
		Task<User?> Get(Guid id, CancellationToken cancellationToken);
		Task Create(User user, CancellationToken cancellationToken);
		Task<bool> Update(User user, CancellationToken cancellationToken);
		Task<bool> Delete(Guid id, CancellationToken cancellationToken);
    }
}