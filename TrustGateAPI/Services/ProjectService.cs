using TrustGateAPI.ModelsDto;
using TrustGateAPI.Repositories.Interfaces;
using TrustGateAPI.Services.Interfaces;

namespace TrustGateAPI.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _repo;

    public ProjectService(IProjectRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<ProjectDto>> GetAllAsync()
    {
        var projects = await _repo.GetAllAsync();

        return projects.Select(p => new ProjectDto
        {
            Id = p.Id,
            Name = p.Name
        }).ToList();
    }

    public async Task<List<ProjectDto>> GetByCompanyIdAsync(int companyId)
    {
        var projects = await _repo.GetByCompanyIdAsync(companyId);

        return projects.Select(p => new ProjectDto
        {
            Id = p.Id,
            Name = p.Name
        }).ToList();
    }
}