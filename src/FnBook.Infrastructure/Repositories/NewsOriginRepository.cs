using FnBook.Domain.Entities;
using FnBook.Domain.Interfaces;
using FnBook.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FnBook.Infrastructure.Repositories;

public class NewsOriginRepository : INewsOriginRepository
{
    private readonly AppDbContext _context;

    public NewsOriginRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<NewsOrigin>> GetAllAsync() =>
        await _context.NewsOrigins.AsNoTracking().ToListAsync();

    public async Task<NewsOrigin?> GetByIdAsync(Guid id) =>
        await _context.NewsOrigins.FindAsync(id);

    public async Task<IEnumerable<NewsOrigin>> GetByNewsIdAsync(Guid newsId) =>
        await _context.NewsOrigins.AsNoTracking()
            .Where(origin => origin.NewsId == newsId)
            .ToListAsync();

    public async Task<NewsOrigin> CreateAsync(NewsOrigin origin)
    {
        _context.NewsOrigins.Add(origin);
        await _context.SaveChangesAsync();
        return origin;
    }

    public async Task<NewsOrigin> UpdateAsync(NewsOrigin origin)
    {
        _context.NewsOrigins.Update(origin);
        await _context.SaveChangesAsync();
        return origin;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var origin = await _context.NewsOrigins.FindAsync(id);
        if (origin is null)
            return false;

        _context.NewsOrigins.Remove(origin);
        await _context.SaveChangesAsync();
        return true;
    }
}
