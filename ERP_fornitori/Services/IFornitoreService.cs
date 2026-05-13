using FornitoriERP.Models;

namespace FornitoriERP.Services
{
    public interface IFornitoreService
    {
        Task<IEnumerable<Fornitore>> GetAllAsync(string? search = null);
        Task<Fornitore?> GetByIdAsync(int id);
        Task<Fornitore> CreateAsync(Fornitore fornitore);
        Task<Fornitore?> UpdateAsync(int id, Fornitore fornitore);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
