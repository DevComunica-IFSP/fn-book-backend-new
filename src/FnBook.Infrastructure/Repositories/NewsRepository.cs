using FnBook.Domain.Entities;
using FnBook.Domain.Interfaces;
using FnBook.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FnBook.Infrastructure.Repositories;

public class NewsRepository : INewsRepository
{
    private readonly AppDbContext _context;

    public NewsRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<News>> GetAllAsync()
    {
        return await _context.News.AsNoTracking().ToListAsync();
    }

    public async Task<News?> GetByIdAsync(Guid id)
    {
        return await _context.News.FindAsync(id);
    }

    public async Task<News> CreateAsync(News news)
    {
        _context.News.Add(news);
        await _context.SaveChangesAsync();
        return news;
    }

    public async Task<News> UpdateAsync(News news)
    {
        _context.News.Update(news);
        await _context.SaveChangesAsync();
        return news;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var news = await _context.News.FindAsync(id);
        if (news is null)
            return false;

        _context.News.Remove(news);
        await _context.SaveChangesAsync();
        return true;
    }
}
