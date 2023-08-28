using Domain.Context;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Domain.Repositories.Auth;

public class AuthRepository : IAuthRepository
{
    private readonly ApplicationDbContext _context;

    public AuthRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserModel?> Login(UserModel user, CancellationToken cancellationToken)
    {
        UserModel? requestedUser = await _context.Users
            .SingleOrDefaultAsync(u => u.Username == user.Username && u.Password == user.Password,
                cancellationToken);

        if (requestedUser is null)
            return null;

        return requestedUser;
    }
}
