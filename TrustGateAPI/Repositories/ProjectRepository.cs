using Microsoft.EntityFrameworkCore;
using TrustGateAPI.Repositories.Interfaces;
using TrustGateCore.Models;
using TrustGateSqlLiteService.Db;

namespace TrustGateAPI.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly SqlDbContext _context;

    public ProjectRepository(SqlDbContext context)
    {
        _context = context;
    }

    public async Task<List<Project>> GetAllAsync()
        => await _context.Projects.ToListAsync();

    public async Task<List<Project>> GetByCompanyIdAsync(int companyId)
        => await _context.Projects
            .Where(p => p.CompanyId == companyId)
            .ToListAsync();

    public async Task<Project?> GetByIdAsync(int id)
        => await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == id);
    
}