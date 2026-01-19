using Microsoft.EntityFrameworkCore;
using TrustGateAPI.Repositories.Interfaces;
using TrustGateCore.Models;
using TrustGateSqlLiteService.Db;

namespace TrustGateAPI.Repositories;

public class CompanyRepository : ICompanyRepository
{
    private readonly SqlDbContext _context;

    public CompanyRepository(SqlDbContext context)
    {
        _context = context;
    }

    public async Task<List<Company>> GetAllAsync()
        => await _context.Companies.ToListAsync();

    public async Task AddAsync(Company company)
    {
        _context.Companies.Add(company);
        await _context.SaveChangesAsync();
    }

    public async Task<Company?> GetByIdAsync(int id)
    => await _context.Companies.FirstOrDefaultAsync(c => c.Id == id);

    public async Task DeleteAsync(Company company)
    {
        _context.Companies.Remove(company);
        await _context.SaveChangesAsync();
    }
}
