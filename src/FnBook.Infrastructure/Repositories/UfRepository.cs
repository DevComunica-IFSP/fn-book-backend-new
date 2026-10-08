using FnBook.Domain.Entities;
using FnBook.Domain.Interfaces;
using FnBook.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FnBook.Infrastructure.Repositories;

public class UfRepository : IUfRepository
{
    private readonly AppDbContext _context;

    public UfRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Uf>> GetAllAsync() =>
        await _context.Ufs.AsNoTracking().OrderBy(uf => uf.Sigla).ToListAsync();

    public async Task<Uf?> GetByIdAsync(Guid id) =>
        await _context.Ufs.FindAsync(id);

    public Task<bool> ExistsBySiglaAsync(string sigla, Guid? excludeId = null) =>
        _context.Ufs.AnyAsync(uf => uf.Sigla == sigla && (!excludeId.HasValue || uf.UfId != excludeId.Value));

    public Task<bool> HasOriginsAsync(Guid id) =>
        _context.NewsOrigins.AnyAsync(origin => origin.UfId == id);

    public async Task<Uf> CreateAsync(Uf uf)
    {
        _context.Ufs.Add(uf);
        await _context.SaveChangesAsync();
        return uf;
    }

    public async Task<Uf> UpdateAsync(Uf uf)
    {
        _context.Ufs.Update(uf);
        await _context.SaveChangesAsync();
        return uf;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var uf = await _context.Ufs.FindAsync(id);
        if (uf is null)
            return false;

        _context.Ufs.Remove(uf);
        await _context.SaveChangesAsync();
        return true;
    }
}
