using Domain.Context;
using Domain.Models;
using Domain.Repositories.User.Interface;
using Microsoft.EntityFrameworkCore;

namespace Domain.Repositories.User.Class;

public sealed class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<UserModel>?> Get(CancellationToken cancellationToken)
    {
        List<UserModel>? users = await _context.Users.ToListAsync(cancellationToken);

        return users is not null ? users : null;
    }

    public async Task<UserModel?> Get(Guid id, CancellationToken cancellationToken)
    {
		UserModel? user = await _context.Users.SingleOrDefaultAsync(u => u.Id == id, cancellationToken);

        return user is not null ? user : null;
    }

    public async Task Create(UserModel user, CancellationToken cancellationToken)
    {
        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> Update(UserModel user, CancellationToken cancellationToken)
    {
		UserModel? requestedUser = await _context.Users.SingleOrDefaultAsync
            (u => u.Id == user.Id, cancellationToken);

        if (requestedUser is null)
            return false;

        _context.Users.Update(requestedUser);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> Delete(Guid id, CancellationToken cancellationToken)
    {
        int affectedRows = await _context.Users.Where(u => u.Id == id)
                                    .ExecuteDeleteAsync(cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return affectedRows > 0 ? true : false;
    }
}
