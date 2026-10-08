using FnBook.Domain.Entities;
using FnBook.Domain.Interfaces;
using FnBook.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FnBook.Infrastructure.Repositories;

public class CommentLikeRepository : ICommentLikeRepository
{
    private readonly AppDbContext _context;

    public CommentLikeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CommentLike>> GetByCommentIdAsync(Guid commentId)
    {
        return await _context.CommentLikes.AsNoTracking()
            .Where(cl => cl.CommentId == commentId)
            .ToListAsync();
    }

    public async Task<bool> ExistsAsync(Guid userId, Guid commentId)
    {
        return await _context.CommentLikes.AnyAsync(cl => cl.UserId == userId && cl.CommentId == commentId);
    }

    public async Task<CommentLike> CreateAsync(CommentLike like)
    {
        _context.CommentLikes.Add(like);
        await _context.SaveChangesAsync();
        return like;
    }

    public async Task<bool> DeleteAsync(Guid userId, Guid commentId)
    {
        var like = await _context.CommentLikes.FindAsync(userId, commentId);
        if (like is null)
            return false;

        _context.CommentLikes.Remove(like);
        await _context.SaveChangesAsync();
        return true;
    }
}
