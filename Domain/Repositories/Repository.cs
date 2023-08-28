using Domain.Context;

namespace Domain.Repositories;

public abstract class Repository<TEntity>
{
	private readonly ApplicationDbContext _context;

	protected Repository(ApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<TEntity> Add(TEntity entity)
	{
		await _context.AddAsync(entity!);

		return entity;
	}
	public async Task<TEntity> Update(TEntity entity)
	{
		_context.Update(entity!);
		await _context.SaveChangesAsync();

		return entity;
	}
	public async Task<TEntity> Delete(TEntity entity)
	{
		_context.Remove(entity!);
		await _context.SaveChangesAsync();

		return entity;
	}
}
