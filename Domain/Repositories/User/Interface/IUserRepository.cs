using Domain.Models;

namespace Domain.Repositories.User.Interface
{
    public interface IUserRepository
    {
        Task<List<UserModel>?> Get(CancellationToken cancellationToken);
        Task<UserModel?> Get(Guid id, CancellationToken cancellationToken);
        Task Create(UserModel user, CancellationToken cancellationToken);
        Task<bool> Update(UserModel user, CancellationToken cancellationToken);
        Task<bool> Delete(Guid id, CancellationToken cancellationToken);
    }
}