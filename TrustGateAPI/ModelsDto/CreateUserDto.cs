using TrustGateAPI.Enums;

namespace TrustGateAPI.ModelsDto;

public class CreateUserDto
{
    public string Name { get; set; } = string.Empty;
    public string Initials { get; set; } = string.Empty;
    public string Login { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public UserRole Role { get; set; }
}
