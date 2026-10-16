using FnBook.Domain.Entities;
using FnBook.Domain.Interfaces;
using FnBook.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FnBook.Infrastructure.Repositories;

public class NewsLikeRepository : INewsLikeRepository
{
    private readonly AppDbContext _context;

    public NewsLikeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<NewsLike>> GetByNewsIdAsync(Guid newsId)
    {
        return await _context.NewsLikes.AsNoTracking()
            .Where(nl => nl.NewsId == newsId)
            .ToListAsync();
    }

    public async Task<bool> ExistsAsync(Guid userId, Guid newsId)
    {
        return await _context.NewsLikes.AnyAsync(nl => nl.UserId == userId && nl.NewsId == newsId);
    }

    public async Task<NewsLike> CreateAsync(NewsLike like)
    {
        _context.NewsLikes.Add(like);
        await _context.SaveChangesAsync();
        return like;
    }

    public async Task<bool> DeleteAsync(Guid userId, Guid newsId)
    {
        var like = await _context.NewsLikes.FindAsync(userId, newsId);
        if (like is null)
            return false;

        _context.NewsLikes.Remove(like);
        await _context.SaveChangesAsync();
        return true;
    }
}
