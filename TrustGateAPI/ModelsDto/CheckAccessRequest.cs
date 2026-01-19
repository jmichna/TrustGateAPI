using System.ComponentModel.DataAnnotations;

namespace TrustGateAPI.ModelsDto;

public class CheckAccessRequest
{
    [Required(ErrorMessage = "Token is required")]
    public string Token { get; set; } = string.Empty;

    [Required(ErrorMessage = "HttpMethod is required")]
    public string HttpMethod { get; set; } = string.Empty;

    [Required(ErrorMessage = "Route is required")]
    public string Route { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "CompanyId must be greater than 0")]
    public int CompanyId { get; set; }
}