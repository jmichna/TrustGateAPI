namespace TrustGateAPI.ModelsDto;

public class ApiTokenListDto
{
    public int Id { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public bool IsActive { get; set; }
    public int ProjectId { get; set; }
}