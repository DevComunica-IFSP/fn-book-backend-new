using FnBook.Domain.Entities;

namespace FnBook.Domain.Interfaces;

public interface IUfRepository
{
    Task<IEnumerable<Uf>> GetAllAsync();
    Task<Uf?> GetByIdAsync(Guid id);
    Task<bool> ExistsBySiglaAsync(string sigla, Guid? excludeId = null);
    Task<bool> HasOriginsAsync(Guid id);
    Task<Uf> CreateAsync(Uf uf);
    Task<Uf> UpdateAsync(Uf uf);
    Task<bool> DeleteAsync(Guid id);
}
