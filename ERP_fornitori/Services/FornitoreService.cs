using Microsoft.EntityFrameworkCore;
using FornitoriERP.Data;
using FornitoriERP.Models;

namespace FornitoriERP.Services
{
    public class FornitoreService : IFornitoreService
    {
        private readonly AppDbContext _context;

        public FornitoreService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Fornitore>> GetAllAsync(string? search = null)
        {
            var query = _context.Fornitori.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower();
                query = query.Where(f =>
                    f.NomeFornitore.ToLower().Contains(search) ||
                    (f.ContattoFornitore != null && f.ContattoFornitore.ToLower().Contains(search)) ||
                    (f.Email != null && f.Email.ToLower().Contains(search)) ||
                    (f.Telefono != null && f.Telefono.Contains(search))
                );
            }

            return await query.OrderBy(f => f.NomeFornitore).ToListAsync();
        }

        public async Task<Fornitore?> GetByIdAsync(int id)
        {
            return await _context.Fornitori.FindAsync(id);
        }

        public async Task<Fornitore> CreateAsync(Fornitore fornitore)
        {
            _context.Fornitori.Add(fornitore);
            await _context.SaveChangesAsync();
            return fornitore;
        }

        public async Task<Fornitore?> UpdateAsync(int id, Fornitore fornitore)
        {
            var existing = await _context.Fornitori.FindAsync(id);
            if (existing == null) return null;

            existing.NomeFornitore = fornitore.NomeFornitore;
            existing.ContattoFornitore = fornitore.ContattoFornitore;
            existing.Telefono = fornitore.Telefono;
            existing.Email = fornitore.Email;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var fornitore = await _context.Fornitori.FindAsync(id);
            if (fornitore == null) return false;

            _context.Fornitori.Remove(fornitore);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Fornitori.AnyAsync(f => f.IdFornitore == id);
        }
    }
}
